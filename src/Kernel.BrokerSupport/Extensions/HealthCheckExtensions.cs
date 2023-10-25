using LTDO.Kernel.BrokerSupport.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace LTDO.Kernel.BrokerSupport.Extensions;

public static class HealthCheckExtensions
{
  public static IHealthChecksBuilder AddRabbitMqCheck(this IHealthChecksBuilder builder)
  {
    return builder.AddCheck<RabbitMqHealthCheck>("RabbitMQ");
  }
}
