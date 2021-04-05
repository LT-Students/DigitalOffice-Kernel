using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Middlewares.ApiInformation
{
    public class ApiInformationMiddleware
    {
        private readonly string _endpoint;

        public ApiInformationMiddleware(
            RequestDelegate next,
            string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            _endpoint = endpoint;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (string.Equals(httpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(httpContext.Request.Path, _endpoint, StringComparison.OrdinalIgnoreCase))
            {
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(
                    BaseApiInfo.GetResponse(),
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));
            }
        }
    }
}
