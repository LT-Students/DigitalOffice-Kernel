using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTDO.Kernel.Responses;

public class OperationResultResponse<T>
{
  public T Body { get; set; }

  [Required]
  public List<string> Errors { get; set; } = new();

  public OperationResultResponse(
    T body = default,
    List<string> errors = default)
  {
    Body = body;
    Errors = errors ?? new();
  }
}
