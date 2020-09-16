using LT.DigitalOffice.Kernel.AccessValidator;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidator.Requests;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKernelExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IAccessValidator, AccessValidator>();

            var rabbitmqOptions = configuration.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>();

            services = ConfigureMassTransit(services, rabbitmqOptions);

            return services;
        }

        private static IServiceCollection ConfigureMassTransit(IServiceCollection services, RabbitMQOptions options)
        {
            services.AddMassTransit(x =>
            {
                x.AddRequestClient<IAccessValidatorUserServiceRequest>(
                    new Uri(options.AccessValidatorUserServiceURL));

                x.AddRequestClient<IAccessValidatorCRServiceRequest>(
                    new Uri(options.AccessValidatorCheckRightsServiceURL));
            });

            services.AddMassTransitHostedService();

            return services;
        }

    }
}