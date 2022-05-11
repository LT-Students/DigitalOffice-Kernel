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
        var coef = 1;

        foreach (var id in guids)
        {
          cache += id.GetHashCode();
        }

        foreach (var arg in additionalArguments)
        {
          if (arg is string)
          {
            //ToDo update cache for string
            foreach (char value in arg.ToString().Replace(" ", ""))
            {
              cache += coef * value.GetHashCode();
              coef++;
            }
          }
          else
          {
            cache += arg.GetHashCode();
          }
        }

        return cache.ToString();
      }
    }

    public static string GetRedisCacheHashCode(this Guid id, params object[] additionalArguments)
    {
      unchecked
      {

        var cache = id.GetHashCode();
        var coef = 1;

        foreach (var arg in additionalArguments)
        {
          if (arg is string)
          {
            //ToDo update cache for string
            foreach (char value in arg.ToString().Replace(" ", ""))
            {
              cache += coef * value.GetHashCode();
              coef++;
            }
          }
          else
          {
            cache += arg.GetHashCode();
          }
        }

        return cache.ToString();
      }
    }
  }
}
