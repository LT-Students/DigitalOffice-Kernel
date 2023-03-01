using DigitalOffice.Kernel.BrokerSupport.TextTemplateModels.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;

namespace DigitalOffice.Kernel.BrokerSupport.Configurations
{
  public class ExtendedBaseRabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
    public virtual string CreateKeywordsEndpoint { get; init; }
  }
}
