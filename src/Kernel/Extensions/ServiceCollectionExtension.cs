using LT.DigitalOffice.Kernel.AccessValidatorEngine;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get all needed types from assembly and inject it in services collection.
        /// </summary>
        public static IServiceCollection InjectObjects(
            this IServiceCollection services,
            InjectObjectType objectType,
            InjectType injectType,
            string assemblyName,
            ILogger logger)
        {
            if (services == null)
            {
                logger?.LogWarning($"Service collection is null, can not '{injectType}' inject '{objectType}s'.");

                return services;
            }

            try
            {
                logger?.LogTrace($"------------------------------------------------------");
                logger?.LogTrace($"Loading assembly '{assemblyName}'.");

                Assembly assembly = Assembly.Load(assemblyName);

                logger?.LogTrace($"Loading '{objectType}s' from '{assemblyName}'.");

                List<Type> injectObjects = assembly.ExportedTypes
                    .Where(
                        t =>
                            t.IsClass
                            && t.IsPublic
                            && t.Name.Contains(objectType.ToString())
                            && t.GetCustomAttribute(typeof(NotAutoInjectAttribute)) == null)
                    .ToList();

                logger?.LogTrace($"Found '{injectObjects.Count}' '{objectType}s'.");

                foreach (var injectObject in injectObjects)
                {
                    Type injectObjectInterface = injectObject.GetInterface($"I{injectObject.Name}");
                    if (injectObjectInterface == null)
                    {
                        logger?.LogWarning($"Can not find interface for '{injectObject.Name}'.");

                        continue;
                    }

                    switch (injectType)
                    {
                        case InjectType.Transient:
                            services.AddTransient(injectObjectInterface, injectObject);
                            break;
                        case InjectType.Scoped:
                            services.AddScoped(injectObjectInterface, injectObject);
                            break;
                        case InjectType.Singletone:
                            services.AddSingleton(injectObjectInterface, injectObject);
                            break;
                    }

                    logger?.LogTrace($"'{injectObject.Name}' was successfuly '{injectType}' injected with interface '{injectObjectInterface.Name}'.");
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, $"Exception while loading types from assembly '{assemblyName}'.");
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
    }
}