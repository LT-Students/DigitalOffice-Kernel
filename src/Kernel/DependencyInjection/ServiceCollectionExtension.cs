using LT.DigitalOffice.Kernel.AccessValidator;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidator.Requests;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using System;

namespace Microsoft.Extensions.DependencyInjection
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
            RabbitMQOptions rabbitmqOptions)
        {
            busConfigurator.AddRequestClient<IAccessValidatorUserServiceRequest>(
                new Uri(rabbitmqOptions.AccessValidatorUserServiceURL));

            busConfigurator.AddRequestClient<IAccessValidatorCheckRightsServiceRequest>(
                new Uri(rabbitmqOptions.AccessValidatorCheckRightsServiceURL));

            return busConfigurator;
        }
    }
}