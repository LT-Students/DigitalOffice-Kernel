using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.DigitalOffice.Kernel.RedisSupport.Extensions
{
  public static class RedisExtension
  {
    public static string GetRedisCacheKey(this IEnumerable<Guid> guids, IEnumerable<(string variableName, object value)> additionalArguments = null)
    {
      StringBuilder sb = new();

      unchecked
      {
        int idsHashCode = 0;

        foreach (Guid id in guids)
        {
          idsHashCode += id.GetHashCode();
        }

        sb.Append(idsHashCode);
      }

      if (additionalArguments is not null)
      {
        foreach ((string variableName, object value) arg in additionalArguments)
        {
          if (arg.value is null)
          {
            continue;
          }

          sb.Append($"{arg.variableName}{arg.value}");
        }
      }

      return sb.ToString();
    }

    public static string GetRedisCacheKey(this Guid id, IEnumerable<(string variableName, object value)> additionalArguments = null)
    {
      StringBuilder sb = new(id.GetHashCode().ToString());

      if (additionalArguments is not null)
      {
        foreach ((string variableName, object value) arg in additionalArguments)
        {
          if (arg.value is null)
          {
            continue;
          }

          sb.Append($"{arg.variableName}{arg.value}");
        }
      }

      return sb.ToString();
    }

    /// <summary>
    /// Returns all properties from passed object exclude collections. Properties are used for redis key creating
    /// </summary>
    /// <param name="obj">Object containing properties</param>
    /// <returns>All properties from passed object exclude collections</returns>
    public static IEnumerable<(string variableName, object value)> GetBasicProperties(this object obj)
    {
      return obj.GetType().GetProperties().Where(p => !p.GetIndexParameters().Any()).Select(x => (variableName: x.Name, value: x.GetValue(obj)))
        .Where(x => !(x.value.GetType().IsAssignableTo(typeof(IEnumerable)) && x.value is not string));
    }
  }
}
