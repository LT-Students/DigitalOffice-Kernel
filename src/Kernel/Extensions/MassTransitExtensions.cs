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
            string assemblyName,
            ILogger logger)
        {
            if (busConfigurator == null)
            {
                return busConfigurator;
            }

            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            if (rabbitMqConfig == null)
            {
                throw new ArgumentNullException(nameof(rabbitMqConfig));
            }

            Assembly assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"Can not load assembly '{assemblyName}'.");
            }

            List<Type> types = assembly.ExportedTypes
                .Where(
                    t =>
                        t.IsInterface
                        && t.IsPublic
                        && t.GetCustomAttribute<AutoInjectRequestAttribute>() != null)
                .ToList();

            foreach (Type type in types)
            {
                var attr = type.GetCustomAttribute<AutoInjectRequestAttribute>();

                PropertyInfo property = rabbitMqConfig
                    .GetType()
                    .GetProperty(attr.EndpointPropertyName);

                if (property == null)
                {
                    logger?.LogError($"Can not find property '{attr.EndpointPropertyName}'.");

                    continue;
                }

                string propertyValue = property.GetValue(rabbitMqConfig)?.ToString();
                if (string.IsNullOrEmpty(propertyValue))
                {
                    logger?.LogError($"RabbitMq config does not contain value for '{attr.EndpointPropertyName}'.");
                }

                Uri endpointUri = new Uri($"{rabbitMqConfig.BaseUrl}/{propertyValue}");

                busConfigurator.AddRequestClient(type, endpointUri, attr.Timeout);

                logger?.LogTrace(
                    $"Found request type '{type.Name}'. Successfully injected with endpoint '{endpointUri}' and timeout '{attr.Timeout.Value}'.");
            }

            return busConfigurator;
        }
    }
}
