using MassTransit;
using System;

namespace LT.DigitalOffice.Kernel.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class AutoInjectRequestAttribute : Attribute
    {
        public string EndpointPropertyName { get; init; }

        public RequestTimeout Timeout { get; init; } = RequestTimeout.Default;

        public AutoInjectRequestAttribute(
            string endpointPropertyName,
            uint timeout = 0)
        {
            EndpointPropertyName = endpointPropertyName;

            if (timeout > 0)
            {
                Timeout = RequestTimeout.After(ms: (int)timeout);
            }
        }
    }
}
