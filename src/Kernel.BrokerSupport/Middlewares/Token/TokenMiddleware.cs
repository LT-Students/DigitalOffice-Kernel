using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Constants;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;

/// <summary>
/// Check JW token middleware.
/// </summary>
public class TokenMiddleware
{
  private const string Token = "token";
  private const string OptionsMethod = "OPTIONS";

  private readonly RequestDelegate requestDelegate;
  private readonly TokenConfiguration tokenConfiguration;

  /// <summary>
  /// Default constructor.
  /// </summary>
  public TokenMiddleware(
    RequestDelegate requestDelegate,
    IOptions<TokenConfiguration> option)
  {
    this.requestDelegate = requestDelegate;

    tokenConfiguration = option.Value;
  }

  /// <summary>
  /// Invoke check token action.
  /// </summary>
  public async Task InvokeAsync(
    HttpContext context,
    IRequestClient<ICheckTokenRequest> client)
  {
    if (string.Equals(context.Request.Method, OptionsMethod, StringComparison.OrdinalIgnoreCase) ||
        tokenConfiguration.SkippedEndpoints != null &&
          tokenConfiguration.SkippedEndpoints.Any(
            url =>
              url.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase)))
    {
      await requestDelegate.Invoke(context);
    }
    else
    {
      var token = context.Request.Headers[Token];

      if (string.IsNullOrEmpty(token))
      {
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return;
      }

      Response<IOperationResult<Guid>> response = null;

      response = await client.GetResponse<IOperationResult<Guid>>(
        ICheckTokenRequest.CreateObj(token),
        timeout: RequestTimeout.After(s: 2));

      if (response.Message.IsSuccess)
      {
        context.Items[ConstStrings.UserId] = response.Message.Body;

        await requestDelegate.Invoke(context);
      }
      else
      {
        context.Response.Headers.AccessControlAllowOrigin = "*";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      }
    }
  }
}
