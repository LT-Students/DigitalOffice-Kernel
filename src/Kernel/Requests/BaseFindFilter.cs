using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.Requests
{
  public record BaseFindFilter
  {
    private int _skipCount;

    [FromQuery(Name = "skipcount")]
    public int SkipCount
    {
      get => _skipCount;
      set => _skipCount = value > -1 ? value : 0;
    }

    private int _takeCount;

    [FromQuery(Name = "takecount")]
    public int TakeCount 
    { 
      get => _takeCount; 
      set => _takeCount = value > 0 ? value : 1;
    }
  }
}
