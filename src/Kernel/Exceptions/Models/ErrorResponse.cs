using System;

namespace LTDO.Kernel.Exceptions.Models;

public class ErrorResponse
{
  public DateTime UtcTime { get; set; }
  public string Header { get; set; }
  public string Message { get; set; }
}
