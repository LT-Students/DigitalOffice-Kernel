using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Configurations;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.Admin;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate.Models;
using LT.DigitalOffice.Kernel.KeywordSupport.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Broker
{
  public static class ServiceEndpointsKeywordsDataHandler
  {
    public static IRequestClient<T> CreateRc<T>(
      this IServiceProvider serviceProvider,
      BaseRabbitMqConfig rabbitConfig,
      string endpoint) where T : class
    {
      return serviceProvider
        .CreateRequestClient<T>(new Uri($"{rabbitConfig.BaseUrl}/{endpoint}"));
    }

    public static async Task<Dictionary<string, Guid>> ProcessEndpoints<TEnum>(
      this IApplicationBuilder app,
      ExtendedBaseRabbitMqConfig rabbitConfig,
      BaseServiceInfoConfig serviceConfig) where TEnum : struct, Enum
    {
      IServiceProvider provider = app
        .ApplicationServices
        .GetRequiredService<IServiceProvider>();

      Dictionary<string, Guid> endpointsIds =
        await RequestHandler.ProcessRequest<ICreateServiceEndpointsRequest, Dictionary<string, Guid>>(
          provider.CreateRc<ICreateServiceEndpointsRequest>(rabbitConfig, rabbitConfig.CreateServiceEndpointsEndpoint),
          ICreateServiceEndpointsRequest.CreateObj(
            serviceName: serviceConfig.Name,
            endpointsNames: Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(v => v.ToString()).ToList()));

      Dictionary<int, List<string>> endpointsKeywords = KeywordCollector
        .GetEndpointKeywords();

      if (endpointsIds is not null && endpointsKeywords is not null)
      {
        List<EndpointKeywords> keywordsRequest = new();

        foreach (var endpointId in endpointsIds)
        {
          if (Enum.TryParse(endpointId.Key, out TEnum serviceEndpoint))
          {
            keywordsRequest.Add(new EndpointKeywords(endpointId.Value, endpointsKeywords[Convert.ToInt32(serviceEndpoint)]));
          }
        }

        if (keywordsRequest.Any())
        {
          await RequestHandler.ProcessRequest<ICreateKeywordsRequest, bool>(
            provider.CreateRc<ICreateKeywordsRequest>(rabbitConfig, rabbitConfig.CreateKeywordsEndpoint),
            ICreateKeywordsRequest.CreateObj(keywordsRequest));
        }
      }

      return endpointsIds;
    }
  }
}
