using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Helpers
{
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
      ILogger logger = null) where U : class
    {
      IOperationResult<T> result = default;

      try
      {
        Response<IOperationResult<T>> response = await requestClient.GetResponse<IOperationResult<T>>(request, timeout: 5000);

        if (!response.IsSuccess())
        {
          errors?.Add("Request was not success.");
        }

        if (response.Message.Errors.Any())
        {
          logger?.LogWarning(
            "Errors while processing request:\n {Errors}",
            string.Join('\n', response.Message.Errors));
        }

        result = response.Message;
      }
      catch (Exception exc)
      {
        logger?.LogError(exc, "Can not process request.");
      }

      return result != null ? result.Body : default;
    }
  }
}
