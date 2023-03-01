using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Extensions
{
  public static class MassTransitExtensions
  {
    private static IServiceCollectionBusConfigurator AddRequestClients(
      this IServiceCollectionBusConfigurator busConfigurator,
      BaseRabbitMqConfig rabbitMqConfiguration)
    {
      if (busConfigurator is null)
      {
        return busConfigurator;
      }

      List<PropertyInfo> propertyInfos = rabbitMqConfiguration.GetType().GetProperties()
        .Where(property => Attribute.IsDefined(property, typeof(AutoInjectRequestAttribute)))
        .ToList();

      foreach (var property in propertyInfos)
      {
        AutoInjectRequestAttribute attr = property.GetCustomAttribute<AutoInjectRequestAttribute>();

        string propertyValue = property.GetValue(rabbitMqConfiguration)?.ToString();

        if (string.IsNullOrEmpty(propertyValue))
        {
          throw new ArgumentNullException(property.Name);
        }

        Uri endpointUri = new Uri($"{rabbitMqConfiguration.BaseUrl}/{propertyValue}");

        busConfigurator.AddRequestClient(attr.Model, endpointUri, attr.Timeout);

        Log.Information(
          $"Found request type '{attr.Model.Name}'. Successfully injected with endpoint '{endpointUri}' and timeout '{attr.Timeout.Value}'.");
      }

      return busConfigurator;
    }

    public static void ConfigureMassTransit<T>(
      this IServiceCollection services,
      T rabbitMQConfiguration)
      where T : BaseRabbitMqConfig
    {
      if (rabbitMQConfiguration is null)
      {
        throw new ArgumentNullException(nameof(rabbitMQConfiguration));
      }

      List<(string queueName, Type consumerType)> endpoints = rabbitMQConfiguration.GetType().GetProperties()
        .Where(property => Attribute.IsDefined(property, typeof(MassTransitEndpointAttribute)))
        .Select((endpoint) =>
        {
          (string queueName, Type consumerType) tuple =
            (queueName: endpoint.GetValue(rabbitMQConfiguration)?.ToString(),
            consumerType: endpoint.GetCustomAttribute<MassTransitEndpointAttribute>()?.ConsumerType);

          if (tuple.queueName is null)
          {
            throw new ArgumentNullException(endpoint.Name);
          }

          if (tuple.consumerType is null)
          {
            throw new ArgumentNullException(nameof(tuple.consumerType), $"{nameof(tuple.consumerType)} is null in {endpoint.Name}.");
          }

          return tuple;
        })
        .ToList();

      services.AddMassTransit(busConfigurator =>
      {
        busConfigurator.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(rabbitMQConfiguration.Host, rabbitMQConfiguration.VirtualHost, configurator =>
          {
            configurator.Username(rabbitMQConfiguration.Username);
            configurator.Password(rabbitMQConfiguration.Password);
          });

          foreach ((string queueName, Type consumerType) endpoint in endpoints)
          {
            busConfigurator.AddConsumer(endpoint.consumerType);
            
            Log.Information($"Consumer '{endpoint.consumerType}' was successfully added.");
          }

          foreach ((string queueName, Type consumerType) endpoint in endpoints)
          {
            cfg.ReceiveEndpoint(endpoint.queueName, ep =>
            {
              ep.ConfigureConsumer(context, endpoint.consumerType);
            });

            Log.Information($"Recieve endpoint '{endpoint.queueName}' for consumer '{endpoint.consumerType}' was successfully configured.");
          }
        });

        busConfigurator.AddRequestClients(rabbitMQConfiguration);
      });
    }
  }
}
