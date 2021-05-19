using FluentValidation;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel
{
    /// <summary>
    /// Represents exception handler middleware. Provides method for handle exceptions.
    /// </summary>
    public static class ExceptionsHandler
    {
        private static void LogError(HttpContext context, ILogger logger)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Exception while processing request to '{context.Request.Path}'.");
            if (context.Request.Query.Any())
            {
                sb.AppendLine("    Query parameters:");
                foreach (KeyValuePair<string, StringValues> parameter in context.Request.Query)
                {
                    sb.AppendLine($"        {parameter.Key}: {parameter.Value}");
                }
            }

            logger.LogError(sb.ToString());
        }

        /// <summary>
        /// Handle exceptions.
        /// </summary>
        /// <param name="context">Specified http context.</param>
        /// <returns>Asynchronous operation for handling exceptions.</returns>
        public static async Task Handle(HttpContext context, ILogger logger)
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            var errorResponse = new ErrorResponse
            {
                UtcTime = DateTime.UtcNow,
            };

            context.Response.ContentType = MediaTypeNames.Application.Json;

            if (exception is BaseException baseException)
            {
                context.Response.StatusCode = baseException.StatusCode;
                errorResponse.Header = baseException.Header;
                errorResponse.Message = baseException.Message;
            }
            else if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Header = HttpStatusCode.BadRequest.ToString();
                errorResponse.Message = validationException.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Header = HttpStatusCode.InternalServerError.ToString();
                errorResponse.Message = "Something wrong.";
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            LogError(context, logger);
        }
    }
}