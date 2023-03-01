using System;
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

    private string host = Environment.GetEnvironmentVariable("RabbitMQHost");
    public string Host
    {
      get
      {
        return host;
      }
      init
      {
        if (host is null)
        {
          host = value;
        }
      }
    }

    private string virtualHost = Environment.GetEnvironmentVariable("RabbitMQVirtualHost");
    public string VirtualHost
    {
      get
      {
        return virtualHost;
      }
      init
      {
        if (virtualHost is null)
        {
          virtualHost = value;
        }
      }
    }

    private string username = Environment.GetEnvironmentVariable("RabbitMQUsername");
    public string Username
    {
      get
      {
        return username;
      }
      init
      {
        if (username is null)
        {
          username = value;
        }
      }
    }

    private string password = Environment.GetEnvironmentVariable("RabbitMQPassword");
    public string Password
    {
      get
      {
        return password;
      }
      init
      {
        if (password is null)
        {
          password = value;
        }
      }
    }

    [AutoInjectRequest(typeof(ICheckUserIsAdminRequest))]
    public virtual string CheckUserIsAdminEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckUserRightsRequest))]
    public virtual string CheckUserRightsEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckUserAnyRightRequest))]
    public virtual string CheckUserAnyRightEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckTokenRequest))]
    public virtual string ValidateTokenEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckProjectManagerRequest))]
    public virtual string CheckProjectManagerEndpoint { get; init; }

    [AutoInjectRequest(typeof(ICheckDepartmentManagerRequest))]
    public virtual string CheckDepartmentManagerEndpoint { get; init; }
  }
}
