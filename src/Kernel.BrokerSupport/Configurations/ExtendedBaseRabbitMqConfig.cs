using DigitalOffice.Kernel.BrokerSupport.Attributes;
using DigitalOffice.Kernel.BrokerSupport.TextTemplateModels.Requests;

namespace DigitalOffice.Kernel.BrokerSupport.Configurations;

public class ExtendedBaseRabbitMqConfig : BaseRabbitMqConfig
{
  [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
  public virtual string CreateKeywordsEndpoint { get; init; }
}
