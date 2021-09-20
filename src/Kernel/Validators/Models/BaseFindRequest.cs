using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.Validators.Models
{
    public record BaseFindRequest
    {
        [FromQuery(Name = "skipcount")]
        public int skipCount { get; set; }

        [FromQuery(Name = "takecount")]
        public int takeCount { get; set; }
    }
}
