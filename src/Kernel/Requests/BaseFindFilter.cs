using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.Requests
{
  public record BaseFindFilter
  {
    [FromQuery(Name = "skipcount")]
    public int SkipCount { get; set; }

    [FromQuery(Name = "takecount")]
    public int TakeCount { get; set; }

    public BaseFindFilter(
      int skipCount = 0,
      int takeCount = 1)
    {
      SkipCount = skipCount;
      TakeCount = takeCount;
    }
  }
}
