using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.Kernel.Requests;

/// <summary>
/// Base filter for find requests.
/// </summary>
public record BaseFindFilter
{
  private int _skipCount;
  private int _takeCount = 0;

  /// <summary>
  /// Number of entries to skip.
  /// </summary>
  [FromQuery(Name = "skipcount")]
  [Required]
  public int SkipCount
  {
    get => _skipCount;
    set => _skipCount = value > -1 ? value : 0;
  }

  /// <summary>
  /// Number of entries to take.
  /// </summary>
  [FromQuery(Name = "takecount")]
  [Required]
  public int TakeCount
  {
    get => _takeCount;
    set => _takeCount = value > -1 ? value : 0;
  }
}
