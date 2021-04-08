using System;

namespace LT.DigitalOffice.Kernel.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoInjectConsumerAttribute : Attribute
    {
        public string EndpointPropertyName { get; init; }

        public AutoInjectConsumerAttribute(
            string endpointPropertyName)
        {
            EndpointPropertyName = endpointPropertyName;
        }
    }
}
