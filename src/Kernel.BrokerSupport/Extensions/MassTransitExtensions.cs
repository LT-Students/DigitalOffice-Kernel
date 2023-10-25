using LTDO.Kernel.BrokerSupport.Attributes;
using LTDO.Kernel.BrokerSupport.Configurations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LTDO.Kernel.BrokerSupport.Extensions;

public static class MassTransitExtensions
{
  private static IBusRegistrationConfigurator AddRequestClients(
    this IBusRegistrationConfigurator busConfigurator,
    BaseRabbitMqConfig rabbitMqConfiguration)
  {
    if (busConfigurator is null)
    {
      return busConfigurator;
    }

    List<PropertyInfo> requestEndpointsInfos = rabbitMqConfiguration.GetType().GetProperties()
      .Where(property => Attribute.IsDefined(property, typeof(AutoInjectRequestAttribute)))
      .ToList();

    foreach (PropertyInfo requestEndpointInfo in requestEndpointsInfos)
    {
      AutoInjectRequestAttribute attr = requestEndpointInfo.GetCustomAttribute<AutoInjectRequestAttribute>();

      string requestEndpoint = requestEndpointInfo.GetValue(rabbitMqConfiguration)?.ToString();

      if (string.IsNullOrEmpty(requestEndpoint))
      {
        throw new ArgumentNullException(requestEndpointInfo.Name);
      }

      Uri endpointUri = new Uri($"{rabbitMqConfiguration.BaseUrl}/{requestEndpoint}");

      busConfigurator.AddRequestClient(attr.Model, endpointUri, attr.Timeout);

      Log.Information(
        "Found request type {0}. Successfully injected with endpoint {1} and timeout {2}.",
        attr.Model.Name,
        requestEndpoint,
        attr.Timeout.Value);
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

    List<(string queueName, Type consumerType)> receiveEndpoints = rabbitMQConfiguration.GetType().GetProperties()
      .Where(property => Attribute.IsDefined(property, typeof(MassTransitEndpointAttribute)))
      .Select((receiveEndpoint) =>
      {
        (string queueName, Type consumerType) tuple =
          (queueName: receiveEndpoint.GetValue(rabbitMQConfiguration)?.ToString(),
          consumerType: receiveEndpoint.GetCustomAttribute<MassTransitEndpointAttribute>()?.ConsumerType);

        if (tuple.queueName is null)
        {
          throw new ArgumentNullException(receiveEndpoint.Name);
        }

        if (tuple.consumerType is null)
        {
          throw new ArgumentNullException(nameof(tuple.consumerType), $"{nameof(tuple.consumerType)} is null in {receiveEndpoint.Name}.");
        }

        return tuple;
      })
      .ToList();

    services.AddMassTransit(busConfigurator =>
    {
      foreach ((string queueName, Type consumerType) receiveEndpoint in receiveEndpoints)
      {
        busConfigurator.AddConsumer(receiveEndpoint.consumerType);
      }

      busConfigurator.UsingRabbitMq((context, cfg) =>
      {
        cfg.Host(rabbitMQConfiguration.Host, rabbitMQConfiguration.VirtualHost, configurator =>
        {
          configurator.Username(rabbitMQConfiguration.Username);
          configurator.Password(rabbitMQConfiguration.Password);
        });

        //configurating receive endpoints
        foreach ((string queueName, Type consumerType) receiveEndpoint in receiveEndpoints)
        {
          cfg.ReceiveEndpoint(receiveEndpoint.queueName, ep =>
          {
            ep.ConfigureConsumer(context, receiveEndpoint.consumerType);
          });
        }
      });

      busConfigurator.AddRequestClients(rabbitMQConfiguration);

      services.Configure<MassTransitHostOptions>(options => { });
    });
  }
}
