using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Enums;

namespace LT.DigitalOffice.Kernel.Responses
{
  public class FindResultResponse<T>
  {
    public List<T> Body { get; set; }
    public int TotalCount { get; set; }
    public List<string> Errors { get; set; } = new();

    public FindResultResponse(
      List<T> body = default,
      int totalCount = default,
      OperationResultStatusType status = default,
      List<string> errors = default)
    {
      Body = body;
      TotalCount = totalCount;
      Errors = errors ?? new();
    }
  }
}
