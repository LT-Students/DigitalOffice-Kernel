using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using LT.DigitalOffice.Kernel.Exceptions;

namespace LT.DigitalOffice.Kernel
{
    /// <summary>
    /// Represents exception handler middleware. Provides method for handle exceptions.
    /// </summary>
    public static class CustomExceptionHandler
    {
        /// <summary>
        /// Handle exceptions.
        /// </summary>
        /// <param name="context">Specified http context.</param>
        /// <returns>Asynchronous operation for handling exceptions.</returns>
        public static async Task HandleCustomException(HttpContext context)
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            
            var errorResponse = new ErrorResponse
            {
                UtcTime = DateTime.UtcNow,
            };

            if (exception is BaseException baseException)
            {
                context.Response.StatusCode = baseException.StatusCode;
                errorResponse.Header = baseException.Header;
                errorResponse.Message = baseException.Message;
            }
            else
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse.Header = "Internal server error";
                errorResponse.Message = exception?.Message;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}