using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.RedisSupport.Extensions
{
  public static class RedisExtension
  {
    public static string GetRedisCacheHashCode(this IEnumerable<Guid> guids, params object[] additionalArguments)
    {
      unchecked
      {
        var cache = 0;

        foreach (var id in guids)
        {
          cache += id.GetHashCode();
        }

        foreach (var arg in additionalArguments)
        {
          cache += arg.GetHashCode();
        }

        return cache.ToString();
      }
    }

    public static string GetRedisCacheHashCode(this Guid id, params object[] additionalArguments)
    {
      unchecked
      {
        var cache = id.GetHashCode();

        foreach (var arg in additionalArguments)
        {
          cache += arg.GetHashCode();
        }

        return cache.ToString();
      }
    }
  }
}
