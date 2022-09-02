using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.Kernel.RedisSupport.Extensions
{
  public static class RedisExtension
  {
    private static int GetStringHashCode(this string arg)
    {
      if (!arg.Any())
      {
        return 0;
      }

      int hash = 17;
      int coef = 31;

      unchecked
      {
        foreach (char ch in arg)
        {
          hash += hash * coef + ch.GetHashCode();
        }
      }

      return hash;
    }

    public static string GetRedisCacheHashCode(this IEnumerable<Guid> guids, params (string variableName, object value)[] additionalArguments)
    {
      unchecked
      {
        int hashCode = 1;

        foreach (Guid id in guids)
        {
          hashCode += id.GetHashCode();
        }

        foreach ((string variableName, object value) arg in additionalArguments)
        {
          if (arg.value is null)
          {
            continue;
          }

          if (arg.value is string)
          {
            hashCode += hashCode * (arg.value.ToString().GetStringHashCode() + arg.variableName.GetStringHashCode());
          }
          else
          {
            hashCode += hashCode * (arg.value.GetHashCode() + arg.variableName.GetStringHashCode());
          }
        }

        return hashCode.ToString();
      }
    }

    public static string GetRedisCacheHashCode(this Guid id, params (string variableName, object value)[] additionalArguments)
    {
      unchecked
      {
        int hashCode = 1 + id.GetHashCode();

        foreach ((string variableName, object value) arg in additionalArguments)
        {
          if (arg.value is null)
          {
            continue;
          }

          if (arg.value is string)
          {
            hashCode += hashCode * (arg.value.ToString().GetStringHashCode() + arg.variableName.GetStringHashCode());
          }
          else
          {
            hashCode += hashCode * (arg.value.GetHashCode() + arg.variableName.GetStringHashCode());
          }
        }

        return hashCode.ToString();
      }
    }
  }
}
