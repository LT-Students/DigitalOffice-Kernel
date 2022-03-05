using System.Collections.Generic;
using LT.DigitalOffice.Kernel.EndpointSupport.Broker.Models.TextTemplate.Models;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Broker.Models.TextTemplate
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
