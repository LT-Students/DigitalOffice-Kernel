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
      set
      {
        if (value > -1)
        {
          _skipCount = value;
        }
      }
    }

    private int _takeCount;

    [FromQuery(Name = "takecount")]
    public int TakeCount 
    { 
      get => _takeCount; 
      set
      {
        if (value > 0)
        {
          _takeCount = value;
        }
      }
    }

    public BaseFindFilter(
      int skipCount = 0,
      int takeCount = 1)
    {
      SkipCount = skipCount;
      TakeCount = takeCount;
    }
  }
}
