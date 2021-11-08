using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.Requests
{
    public record BaseFindFilter
    {
        [FromQuery(Name = "skipcount")]
        public int SkipCount { get; set; }

        [FromQuery(Name = "takecount")]
        public int TakeCount { get; set; }
    }
}
