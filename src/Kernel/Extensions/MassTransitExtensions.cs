using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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

            var asmPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var files = Directory.GetFiles(asmPath, "*DigitalOffice*.dll");

            List<Assembly> assemblies = new();

            foreach (string fileName in files)
            {
                assemblies.Add(Assembly.LoadFrom(fileName));
            }

            List<Type> injectInterfaces = new();

            foreach (Assembly assembly in assemblies)
            {
                injectInterfaces.AddRange(assembly.ExportedTypes
                    .Where(
                        t =>
                            t.IsInterface
                            && t.IsPublic
                            && t.GetCustomAttribute(typeof(AutoInjectRequestAttribute)) != null)
                    .ToList());
            }

            foreach (Type injectInterface in injectInterfaces)
            {
                var attr = injectInterface.GetCustomAttribute<AutoInjectRequestAttribute>();

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

                busConfigurator.AddRequestClient(injectInterface, endpointUri, attr.Timeout);

                logger?.LogTrace(
                    $"Found request type '{injectInterface.Name}'. Successfully injected with endpoint '{endpointUri}' and timeout '{attr.Timeout.Value}'.");
            }

            return busConfigurator;
        }
    }
}
