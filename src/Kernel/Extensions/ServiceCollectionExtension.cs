using LT.DigitalOffice.Kernel.AccessValidatorEngine;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.HealthChecks;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.Extensions
{
    /// <summary>
    /// Helper class for services extensions.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add <see cref="AccessValidator"/> and HttpContextAccessor.
        /// </summary>
        public static IServiceCollection AddKernelExtensions(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<IAccessValidator, AccessValidator>();

            return services;
        }

        /// <summary>
        /// Add requst clients used by <see cref="AccessValidator"/>.
        /// </summary>
        public static IServiceCollectionBusConfigurator ConfigureKernelMassTransit(
            this IServiceCollectionBusConfigurator busConfigurator,
            BaseRabbitMqConfig rabbitmqOptions)
        {
            busConfigurator.AddRequestClient<ICheckUserIsAdminRequest>(
                new Uri($"{rabbitmqOptions.BaseUrl}/{rabbitmqOptions.CheckUserIsAdminEndpoint}"));

            busConfigurator.AddRequestClient<ICheckUserRightsRequest>(
                new Uri($"{rabbitmqOptions.BaseUrl}/{rabbitmqOptions.CheckUserRightsEndpoint}"));

            return busConfigurator;
        }

        public static IServiceCollection AddBusinessObjects(
            this IServiceCollection services,
            ILogger logger = null)
        {
            if (services == null)
            {
                logger?.LogWarning($"Service collection is null, can not inject business objects.");

                return services;
            }

            try
            {
                var asmPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                var files = Directory.GetFiles(asmPath, "*DigitalOffice*.dll");

                List<Assembly> assemblies = new();

                foreach (string fileName in files)
                {
                    assemblies.Add(Assembly.LoadFrom(fileName));
                }

                foreach (Assembly assembly in assemblies)
                {
                    List<Type> injectInterfaces = assembly.ExportedTypes
                        .Where(
                            t =>
                                t.IsInterface
                                && t.IsPublic
                                && t.GetCustomAttribute(typeof(AutoInjectAttribute)) != null)
                        .ToList();

                    foreach (Type injectInterface in injectInterfaces)
                    {
                        var injectObjects = assembly.GetExportedTypes().Where(t => t.GetInterface(injectInterface.Name) != null).ToList();
                        if (!injectObjects.Any())
                        {
                            logger.LogWarning($"No classes were found that inherit the interface '{injectInterface.Name}'.");
                            continue;
                        }

                        if (injectObjects.Count > 1)
                        {
                            logger?.LogWarning(
                                $"Found more than one class '{string.Join(',', injectObjects.Select(t => t.Name))}' inheriting the interface '{injectInterface.Name}'.");

                            continue;
                        }

                        AutoInjectAttribute attr = injectInterface.GetCustomAttribute<AutoInjectAttribute>();
                        switch (attr.InjectType)
                        {
                            case InjectType.Transient:
                                services.AddTransient(injectInterface, injectObjects[0]);
                                break;
                            case InjectType.Scoped:
                                services.AddScoped(injectInterface, injectObjects[0]);
                                break;
                            case InjectType.Singletone:
                                services.AddSingleton(injectInterface, injectObjects[0]);
                                break;
                        }

                        logger?.LogTrace(
                            $"'{injectObjects[0].Name}' was successfuly '{attr.InjectType}' injected with interface '{injectInterface.Name}'.");
                    }
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, $"Exception while loading types from assemblies.");
            }

            return services;
        }

        public static IApplicationBuilder UseExceptionsHandler(
            this IApplicationBuilder app,
            ILoggerFactory loggerFactory)
        {
            if (app == null)
            {
                return app;
            }

            app.UseExceptionHandler(tempApp => tempApp.Run(async context =>
            {
                await ExceptionsHandler.Handle(context, loggerFactory.CreateLogger("Extensions"));
            }));

            return app;
        }

        public static IApplicationBuilder UseApiInformation(
            this IApplicationBuilder app,
            string endpoint = null)
        {
            if (app == null)
            {
                return app;
            }

            string mappedEndpoint = "/apiinformation";
            if (!string.IsNullOrEmpty(endpoint))
            {
                mappedEndpoint = endpoint;
            }

            app.UseMiddleware<ApiInformationMiddleware>(mappedEndpoint);

            return app;
        }

        public static IHealthChecksBuilder AddRabbitMqCheck(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<RabbitMqHealthCheck>("RabbitMQ");
        }
    }
}