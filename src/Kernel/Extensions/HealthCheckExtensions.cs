using LT.DigitalOffice.Kernel.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.Kernel.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddRabbitMqCheck(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<RabbitMqHealthCheck>("RabbitMQ");
        }
    }
}
