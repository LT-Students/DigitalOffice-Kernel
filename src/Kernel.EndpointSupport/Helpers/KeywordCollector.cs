using LT.DigitalOffice.Kernel.EndpointSupport.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Helpers;

public static class KeywordCollector
{
  public static Dictionary<Guid, List<string>> GetEndpointKeywords()
  {
    Dictionary<Guid, List<string>> endpointsKeywords = new();

    IEnumerable<Type> assemblyTargets = AppDomain.CurrentDomain
      .GetAssemblies()
      .SingleOrDefault(assembly => assembly.GetName().Name.EndsWith("Models.Db"))
      .ExportedTypes;

    foreach (Type dbModel in assemblyTargets)
    {
      IEnumerable<PropertyInfo> properties = dbModel
        .GetProperties()
        .Where(p => p.GetCustomAttributes(typeof(KeywordAttribute), true).Any());

      foreach (PropertyInfo property in properties)
      {
        foreach (Guid endpointId in
          (property.GetCustomAttributes(typeof(KeywordAttribute), true).FirstOrDefault() as KeywordAttribute).Endpoints)
        {
          if (!endpointsKeywords.ContainsKey(endpointId))
          {
            endpointsKeywords.Add(endpointId, new List<string>());
          }

          endpointsKeywords[endpointId].Add(property.Name);
        }
      }
    }

    return endpointsKeywords;
  }
}
