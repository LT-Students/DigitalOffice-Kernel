using LTDO.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LTDO.Kernel.BrokerSupport.Attributes;
using LTDO.Kernel.BrokerSupport.Middlewares.Token;
using System;

namespace LTDO.Kernel.BrokerSupport.Configurations;

/// <summary>
/// Base configuration class for RabbitMQ.
/// </summary>
public class BaseRabbitMqConfig
{
  public const string SectionName = "RabbitMQ";

  private const string RabbitMqProtocol = "rabbitmq";

  public string BaseUrl => $"{RabbitMqProtocol}://{Host}";

  private string _host = Environment.GetEnvironmentVariable("RabbitMQ_Host");
  public string Host
  {
    get
    {
      return _host;
    }
    init
    {
      if (string.IsNullOrWhiteSpace(_host))
      {
        _host = value;
      }
    }
  }

  private string _virtualHost = Environment.GetEnvironmentVariable("RabbitMQ_VirtualHost");
  public string VirtualHost
  {
    get
    {
      return _virtualHost;
    }
    init
    {
      if (string.IsNullOrWhiteSpace(_virtualHost))
      {
        _virtualHost = value;
      }
    }
  }

  private string _username = Environment.GetEnvironmentVariable("RabbitMQ_Username");
  public string Username
  {
    get
    {
      return _username;
    }
    init
    {
      if (string.IsNullOrWhiteSpace(_username))
      {
        _username = value;
      }
    }
  }

  private string _password = Environment.GetEnvironmentVariable("RabbitMQ_Password");
  public string Password
  {
    get
    {
      return _password;
    }
    init
    {
      if (string.IsNullOrWhiteSpace(_password))
      {
        _password = value;
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
