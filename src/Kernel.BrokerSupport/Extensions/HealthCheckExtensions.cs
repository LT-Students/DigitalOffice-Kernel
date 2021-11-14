using LT.DigitalOffice.Kernel.BrokerSupport.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Extensions
{
  public static class HealthCheckExtensions
  {
    public static IHealthChecksBuilder AddRabbitMqCheck(this IHealthChecksBuilder builder)
    {
      return builder.AddCheck<RabbitMqHealthCheck>("RabbitMQ");
    }
  }
}
