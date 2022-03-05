using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Broker.Models.TextTemplate.Models
{
  public class EndpointKeywords
  {
    public Guid EndpointId { get; }
    public List<string> Keywords { get; }

    public EndpointKeywords(Guid endpointId, List<string> keywords)
    {
      EndpointId = endpointId;
      Keywords = keywords;
    }
  }
}
