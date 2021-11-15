using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Extensions
{
  public static class MassTransitExtensions
  {
    public static IServiceCollectionBusConfigurator AddRequestClients(
      this IServiceCollectionBusConfigurator busConfigurator,
      BaseRabbitMqConfig rabbitMqConfig,
      ILogger logger = null)
    {
      if (busConfigurator == null)
      {
        return busConfigurator;
      }

      if (rabbitMqConfig == null)
      {
        throw new ArgumentNullException(nameof(rabbitMqConfig));
      }

      var propertyInfos =
        rabbitMqConfig
          .GetType()
          .GetProperties()
          .ToList();

      foreach (var property in propertyInfos)
      {
        var attr = property.GetCustomAttribute<Attributes.AutoInjectRequestAttribute>();

        if (attr is null)
        {
          continue;
        }

        var propertyValue = property.GetValue(rabbitMqConfig)?.ToString();
        if (string.IsNullOrEmpty(propertyValue))
        {
          logger?.LogError($"RabbitMq config does not contain value for '{property.Name}'.");
          continue;
        }

        var endpointUri = new Uri($"{rabbitMqConfig.BaseUrl}/{propertyValue}");

        busConfigurator.AddRequestClient(attr.Model, endpointUri, attr.Timeout);

        logger?.LogTrace(
          $"Found request type '{attr.Model.Name}'. Successfully injected with endpoint '{endpointUri}' and timeout '{attr.Timeout.Value}'.");
      }

      return busConfigurator;
    }
  }
}
