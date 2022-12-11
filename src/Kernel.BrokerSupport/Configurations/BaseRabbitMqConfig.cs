using DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Configurations
{
  /// <summary>
  /// Base configuration class for RabbitMQ.
  /// </summary>
  public class BaseRabbitMqConfig
  {
    public const string SectionName = "RabbitMQ";

    private const string RabbitMqProtocol = "rabbitmq";

    public string BaseUrl => $"{RabbitMqProtocol}://{Host}";

    public string Host { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }

    [AutoInjectRequest(typeof(ICheckUserIsAdminRequest))]
    public string CheckUserIsAdminEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckUserRightsRequest))]
    public string CheckUserRightsEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckUserAnyRightRequest))]
    public string CheckUserAnyRightEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckTokenRequest))]
    public string ValidateTokenEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckProjectManagerRequest))]
    public string CheckProjectManagerEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckDepartmentManagerRequest))]
    public string CheckDepartmentManagerEndpoint { get; init; }
  }
}
