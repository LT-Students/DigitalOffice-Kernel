using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Broker.Models.Admin
{
  public interface ICreateServiceEndpointsRequest
  {
    string ServiceName { get; }
    List<string> EndpointsNames { get; }

    static object CreateObj(string serviceName, List<string> endpointsNames)
    {
      return new
      {
        ServiceName = serviceName,
        EndpointsNames = endpointsNames
      };
    }
  }
}
