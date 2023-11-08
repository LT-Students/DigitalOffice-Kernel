using DigitalOffice.Kernel.Exceptions;
using DigitalOffice.Kernel.Middlewares.ApiInformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace DigitalOffice.Kernel.Extensions;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseExceptionsHandler(
    this IApplicationBuilder app,
    ILoggerFactory loggerFactory)
  {
    if (app is null)
    {
      return app;
    }

    app.UseExceptionHandler(tempApp => tempApp.Run(async context =>
    {
      await ExceptionsHandler.Handle(context, loggerFactory.CreateLogger("Extensions"));
    }));

    return app;
  }

  public static IApplicationBuilder UseApiInformation(
    this IApplicationBuilder app,
    string endpoint = null)
  {
    if (app is null)
    {
      return app;
    }

    string mappedEndpoint = "/apiinformation";
    if (!string.IsNullOrEmpty(endpoint))
    {
      mappedEndpoint = endpoint;
    }

    app.UseMiddleware<ApiInformationMiddleware>(mappedEndpoint);

    return app;
  }
}
