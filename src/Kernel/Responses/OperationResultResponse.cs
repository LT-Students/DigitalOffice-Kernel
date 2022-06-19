using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.Kernel.Responses
{
  public class OperationResultResponse<T>
  {
    public T Body { get; set; }
    public List<string> Errors { get; set; } = new();

    public OperationResultResponse(
      T body = default,
      List<string> errors = default)
    {
      Body = body;
      Errors = errors ?? new();
    }
  }
}
