using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.Requests
{
  public record BaseFindFilter
  {
    private int skipCount;
    private int takeCount;

    [FromQuery(Name = "skipcount")]
    public int SkipCount
    {
      get => skipCount;
      set => skipCount = value > -1 ? value : 0;
    }

    [FromQuery(Name = "takecount")]
    public int TakeCount 
    { 
      get => takeCount; 
      set => takeCount = value > 0 ? value : 1;
    }
  }
}
