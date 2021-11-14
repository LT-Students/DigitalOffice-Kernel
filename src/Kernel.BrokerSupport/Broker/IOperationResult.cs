using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Broker
{
  /// <summary>
  /// Interface that is needed to form the response in message broker.
  /// </summary>
  public interface IOperationResult<T>
  {
    bool IsSuccess { get; }

    List<string> Errors { get; }

    T Body { get; }

    static object CreateObj(bool isSuccess, T body = default, List<string> errors = null)
    {
      return new
      {
        IsSuccess = isSuccess,
        Body = body,
        Errors = errors
      };
    }
  }
}
