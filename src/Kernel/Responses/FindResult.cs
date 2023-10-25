using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTDO.Kernel.Responses;

public class FindResult<T>
{
  public List<T> Body { get; set; }

  [Required]
  public int TotalCount { get; set; }

  public FindResult(
    List<T> body = default,
    int totalCount = default)
  {
    Body = body;
    TotalCount = totalCount;
  }
}
