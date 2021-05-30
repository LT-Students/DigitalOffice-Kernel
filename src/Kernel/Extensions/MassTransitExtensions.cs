using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.Extensions
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

            List<PropertyInfo> propertyInfos =
                rabbitMqConfig
                    .GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(AutoInjectRequestAttribute)) != null).ToList();

            foreach(var property in propertyInfos)
            {
                var attr = property.GetCustomAttribute<AutoInjectRequestAttribute>();

                Uri endpointUri = new Uri($"{rabbitMqConfig.BaseUrl}/{property.GetValue(rabbitMqConfig)}");

                busConfigurator.AddRequestClient(attr.Model, endpointUri, attr.Timeout);

                logger?.LogTrace(
                    $"Found request type '{attr.Model.Name}'. Successfully injected with endpoint '{endpointUri}' and timeout '{attr.Timeout.Value}'.");
            }

            return busConfigurator;
        }
    }
}
