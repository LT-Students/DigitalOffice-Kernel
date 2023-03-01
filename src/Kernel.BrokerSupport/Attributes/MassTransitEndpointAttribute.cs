using System;

namespace DigitalOffice.Kernel.BrokerSupport.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class MassTransitEndpointAttribute : Attribute
  {
    public Type ConsumerType { get; }

    public MassTransitEndpointAttribute(Type consumerType)
    {
      ConsumerType = consumerType;
    }
  }
}
