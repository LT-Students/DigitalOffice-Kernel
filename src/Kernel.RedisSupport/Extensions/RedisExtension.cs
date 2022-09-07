using System;
using System.Collections.Generic;
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
  }
}
