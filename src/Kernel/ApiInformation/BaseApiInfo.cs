using System;

namespace LT.DigitalOffice.Kernel.ApiInformation
{
    public abstract class BaseApiInfo
    {
        public abstract string Version { get; init; }
        public abstract DateTime StartTime { get; init; }
    }
}
