using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Helpers;

public static class ApplicationRequestClientCreator
{
  public static IRequestClient<T> CreateRc<T>(
    this IApplicationBuilder app,
    BaseRabbitMqConfig rabbitConfig,
    string endpoint) where T : class
  {
    IServiceProvider serviceProvider = app
      .ApplicationServices
      .GetRequiredService<IServiceProvider>();

    return serviceProvider
      .CreateRequestClient<T>(new Uri($"{rabbitConfig.BaseUrl}/{endpoint}"));
  }
}
