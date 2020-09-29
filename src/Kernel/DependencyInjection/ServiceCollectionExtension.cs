using LT.DigitalOffice.Kernel.AccessValidator;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidator.Requests;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKernelExtensions(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IAccessValidator, AccessValidator>();

            return services;
        }

        public static IServiceCollectionBusConfigurator ConfigureKernelMassTransit(
            this IServiceCollectionBusConfigurator busConfigurator,
            RabbitMQOptions rabbitmqOptions)
        {
            busConfigurator.AddRequestClient<IAccessValidatorUserServiceRequest>(
                new Uri(rabbitmqOptions.AccessValidatorUserServiceURL));

            busConfigurator.AddRequestClient<IAccessValidatorCRServiceRequest>(
                new Uri(rabbitmqOptions.AccessValidatorCheckRightsServiceURL));

            return busConfigurator;
        }
    }
}