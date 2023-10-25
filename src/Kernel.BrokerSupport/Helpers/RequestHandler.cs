using LTDO.Kernel.BrokerSupport.Broker;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LTDO.Kernel.BrokerSupport.Helpers;

public static class RequestHandler
{
  public static bool IsSuccess<T>(this Response<IOperationResult<T>> response)
  {
    return response != null && response.Message.IsSuccess;
  }

  public static async Task<T> ProcessRequest<U, T>(
    this IRequestClient<U> requestClient,
    object request,
    List<string> errors = null,
    ILogger logger = null,
    int? timeout = 5000,
    CancellationToken ct = default) where U : class
  {
    IOperationResult<T> result = default;

    try
    {
      Response<IOperationResult<T>> response = await requestClient.GetResponse<IOperationResult<T>>(
        values: request,
        cancellationToken: ct,
        timeout: RequestTimeout.After(ms: timeout));

      if (!response.IsSuccess())
      {
        errors?.Add("Request was not success.");

        if (response.Message.Errors.Any())
        {
          logger?.LogWarning(
            "Errors while processing request:\n {Errors}",
            string.Join('\n', response.Message.Errors));
        }
      }

      result = response.Message;
    }
    catch (Exception exc)
    {
      logger?.LogError(exc, $"Can not process request {typeof(U).FullName}.");
    }

    return result != null ? result.Body : default;
  }
}
