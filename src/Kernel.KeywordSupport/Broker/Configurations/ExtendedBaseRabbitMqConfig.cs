using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.EndpointSupport.Broker.Models.TextTemplate;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Broker.Configurations
{
  public class ExtendedBaseRabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
    public string CreateKeywordsEndpoint { get; init; }
  }
}
