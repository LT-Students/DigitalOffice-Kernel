using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LT.DigitalOffice.Kernel.KeywordSupport.Attributes;

namespace LT.DigitalOffice.Kernel.KeywordSupport.Helpers
{
  public static class KeywordCollector
  {
    public static Dictionary<int, List<string>> GetEndpointKeywords()
    {
      Dictionary<int, List<string>> endpointsKeywords = new();

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
          foreach (int endpoint in
            (property.GetCustomAttributes(typeof(KeywordAttribute), true).FirstOrDefault() as KeywordAttribute).Endpoints)
          {
            if (!endpointsKeywords.ContainsKey(endpoint))
            {
              endpointsKeywords.Add(endpoint, new List<string>());
            }

            endpointsKeywords[endpoint].Add(property.Name);
          }
        }
      }

      return endpointsKeywords;
    }
  }
}
