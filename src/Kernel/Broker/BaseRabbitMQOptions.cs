using LT.DigitalOffice.Kernel.AccessValidatorEngine;
using System;

namespace LT.DigitalOffice.Kernel.Broker
{
  /// <summary>
  /// Base configuration class for RabbitMQ.
  /// </summary>
  public class BaseRabbitMqOptions
  {
    private string _host;

    private const string RabbitMqProtocol = "rabbitmq";

    /// <summary>
    /// Section name.
    /// </summary>
    public const string RabbitMqSectionName = "RabbitMQ";

    /// <summary>
    /// RabbitMQ host address.
    /// </summary>
    public string Host
    {
      get
      {
        return $"{RabbitMqProtocol}://{_host}";
      }
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          throw new ArgumentNullException(nameof(Host));
        }
        _host = value;
      }
    }

    /// <summary>
    /// RabbitMQ username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// RabbitMQ password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// RabbitMQ UserService channel url used by <see cref="AccessValidator"/>.
    /// </summary>
    public string UserServiceCheckUserIsAdminEndpoint { get; set; }

    /// <summary>
    /// RabbitMQ CheckRightsService channel url used by <see cref="AccessValidator"/>.
    /// </summary>
    public string CRServiceCheckUserRightsEndpoint { get; set; }
  }
}