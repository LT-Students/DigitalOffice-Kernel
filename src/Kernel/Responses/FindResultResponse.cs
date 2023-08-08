using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.Kernel.Responses;

public class FindResultResponse<T>
{
  public List<T> Body { get; set; }

  [Required]
  public int TotalCount { get; set; }

  [Required]
  public List<string> Errors { get; set; } = new();

  public FindResultResponse(
    List<T> body = default,
    int totalCount = default,
    List<string> errors = default)
  {
    Body = body;
    TotalCount = totalCount;
    Errors = errors ?? new();
  }
}
