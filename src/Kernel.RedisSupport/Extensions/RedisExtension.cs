using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTDO.Kernel.RedisSupport.Extensions;

public static class RedisExtension
{
  public static string GetRedisCacheKey(this IEnumerable<Guid> guids, string requestName, IEnumerable<(string variableName, object value)> additionalArguments = null)
  {
    StringBuilder sb = new(requestName);

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

  public static string GetRedisCacheKey(this Guid id, string requestName, IEnumerable<(string variableName, object value)> additionalArguments = null)
  {
    StringBuilder sb = new StringBuilder(requestName).Append(id.GetHashCode().ToString());

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
  /// <returns>An <see cref="IEnumerable{T}"/> that contains tuple with properties'
  /// <see cref="string"/> name and <see cref="object"/> value from
  /// passed object exclude collections, <see langword="null" /> if obj is null</returns>
  public static IEnumerable<(string variableName, object value)> GetBasicProperties(this object obj)
  {
    return obj?.GetType().GetProperties().Where(p => !p.GetIndexParameters().Any()).Select(x => (variableName: x.Name, value: x.GetValue(obj)))
      .Where(x => x.value is not null && (!x.value.GetType().IsAssignableTo(typeof(IEnumerable)) || x.value is string));
  }
}
