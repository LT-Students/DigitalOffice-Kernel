using LTDO.Kernel.BrokerSupport.Attributes;
using LTDO.Kernel.BrokerSupport.TextTemplateModels.Requests;

namespace LTDO.Kernel.BrokerSupport.Configurations;

public class ExtendedBaseRabbitMqConfig : BaseRabbitMqConfig
{
  [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
  public virtual string CreateKeywordsEndpoint { get; init; }
}
