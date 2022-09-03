using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.DigitalOffice.Kernel.RedisSupport.Extensions
{
  public static class RedisExtension
  {
    public static string GetRedisCacheHashCode(this IEnumerable<Guid> guids, params (string variableName, object value)[] additionalArguments)
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

      foreach ((string variableName, object value) arg in additionalArguments)
      {
        if (arg.value is null)
        {
          continue;
        }

        sb.Append($"{arg.variableName}{arg.value}");
      }

      return sb.ToString();
    }

    public static string GetRedisCacheHashCode(this Guid id, params (string variableName, object value)[] additionalArguments)
    {
      StringBuilder sb = new(id.GetHashCode().ToString());

      foreach ((string variableName, object value) arg in additionalArguments)
      {
        if (arg.value is null)
        {
          continue;
        }

        sb.Append($"{arg.variableName}{arg.value}");
      }

      return sb.ToString();
    }
  }
}
