using System.Collections.Generic;

namespace LTDO.Kernel.BrokerSupport.TextTemplateModels.Requests;

public interface ICreateKeywordsRequest
{
  List<EndpointKeywords> EndpointsKeywords { get; }

  static object CreateObj(List<EndpointKeywords> endpointsKeywords)
  {
    return new
    {
      EndpointsKeywords = endpointsKeywords
    };
  }
}
