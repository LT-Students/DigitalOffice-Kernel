using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.Admin;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Broker.Configurations
{
  public class ExtendedBaseRabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(ICreateServiceEndpointsRequest))]
    public string CreateServiceEndpointsEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
    public string CreateKeywordsEndpoint { get; init; }
  }
}
