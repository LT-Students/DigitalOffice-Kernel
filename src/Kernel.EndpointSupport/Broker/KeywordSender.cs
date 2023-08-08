using DigitalOffice.Kernel.BrokerSupport.Configurations;
using DigitalOffice.Kernel.BrokerSupport.TextTemplateModels;
using DigitalOffice.Kernel.BrokerSupport.TextTemplateModels.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.EndpointSupport.Helpers;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Broker;

public static class KeywordSender
{
  public static async Task Send(
    this IApplicationBuilder app,
    ExtendedBaseRabbitMqConfig rabbitConfig)
  {
    Dictionary<Guid, List<string>> endpointsKeywords = KeywordCollector
      .GetEndpointKeywords();

    if (endpointsKeywords.Any())
    {
      List<EndpointKeywords> requestData = new();

      foreach (var endpointKeywords in endpointsKeywords)
      {
        requestData.Add(
          new EndpointKeywords(
            endpointId: endpointKeywords.Key,
            keywords: endpointKeywords.Value));
      }

      await RequestHandler.ProcessRequest<ICreateKeywordsRequest, bool>(
        app.CreateRc<ICreateKeywordsRequest>(rabbitConfig, rabbitConfig.CreateKeywordsEndpoint),
        ICreateKeywordsRequest.CreateObj(requestData));
    }
  }
}
