using LT.DigitalOffice.Kernel.Broker;
using Microsoft.Extensions.Configuration;
using LT.DigitalOffice.Kernel.AccessValidator;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKernelExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IAccessValidator, AccessValidator>();
            services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.RabbitMQ));

            return services;
        }
    }
}