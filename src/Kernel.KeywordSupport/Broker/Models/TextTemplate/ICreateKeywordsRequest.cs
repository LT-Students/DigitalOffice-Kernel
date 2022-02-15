using System.Collections.Generic;
using LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate.Models;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate
{
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
}
