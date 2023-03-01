using System;
using System.Collections.Generic;

namespace DigitalOffice.Kernel.BrokerSupport.TextTemplateModels
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
