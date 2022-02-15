using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.TextTemplate.Models
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
