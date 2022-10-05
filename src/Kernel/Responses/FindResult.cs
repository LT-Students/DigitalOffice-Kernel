using System.Collections.Generic;

namespace DigitalOffice.Kernel.Responses;

public class FindResult<T>
{
  public List<T> Body { get; set; }
  public int TotalCount { get; set; }

  public FindResult(
    List<T> body = default,
    int totalCount = default)
  {
    Body = body;
    TotalCount = totalCount;
  }
}
