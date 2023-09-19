using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.Kernel.Requests;

public record BaseFindFilter
{
  private int skipCount;
  private int takeCount = 0;

  /// <summary>
  /// Number of entries to skip.
  /// </summary>
  [FromQuery(Name = "skipcount")]
  [Required]
  public int SkipCount
  {
    get => skipCount;
    set => skipCount = value > -1 ? value : 0;
  }

  /// <summary>
  /// Number of entries to take.
  /// </summary>
  [FromQuery(Name = "takecount")]
  [Required]
  public int TakeCount
  {
    get => takeCount;
    set => takeCount = value > -1 ? value : 0;
  }
}
