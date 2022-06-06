using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LT.DigitalOffice.Kernel.Extensions
{
  public static class StringExtensions
  {
    public static string EncodeSqlConnectionString(this string str)
    {
      if (string.IsNullOrEmpty(str) || !str.Contains(';'))
      {
        return str;
      }

      string[] values = str.Split(';');
      if (values.Length < 3)
      {
        return str;
      }

      StringBuilder result = new();

      foreach (string value in values)
      {
        string[] subValues = value.Split('=');
        if (subValues.Length != 2)
        {
          result.Append(value);

          continue;
        }

        if (subValues[0].ToUpper() == "USER" ||
            subValues[0].ToUpper() == "PASSWORD")
        {
          subValues[1] = "*****";

          result.Append($"{subValues[0]}={subValues[1]};");

          continue;
        }

        result.Append($"{value};");
      }

      return result.ToString();
    }

    public static string ToServiceUpTime(this DateTime time)
    {
      TimeSpan difference = DateTime.UtcNow - time;

      return $"{difference.Days} days {difference.Hours}h {difference.Minutes}m {difference.Seconds}s";
    }

    public static T TrimSpaces<T>(this T obj, Type modelType)
    {
      IEnumerable<PropertyInfo> properties = modelType
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);

      foreach (PropertyInfo property in properties)
      {
        string value = (string)property.GetValue(obj);
        if (!string.IsNullOrWhiteSpace(value))
        {
          var newValue = (object)value.Trim();
          property.SetValue(obj, newValue);
        }
      }

      Type baseTypeInfo = obj.GetType().BaseType;
      if (baseTypeInfo != null && baseTypeInfo.FullName.Contains("List"))
      {
        int listCount = (int)obj.GetType().GetProperty("Count").GetValue(obj, null);
        for (int innerIndex = 0; innerIndex < listCount; innerIndex++)
        {
          object item = obj.GetType()
            .GetMethod("get_Item", new Type[] { typeof(int) })
            .Invoke(obj, new object[] { innerIndex });
          item.TrimSpaces(modelType);
        }
      }

      IEnumerable<PropertyInfo> customTypes =
        obj.GetType()
          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(prop =>
              !prop.GetType().IsPrimitive && prop.GetType().IsClass &&
              !prop.PropertyType.FullName.StartsWith("System"));

      foreach (PropertyInfo customType in customTypes)
      {
        if (customType.GetIndexParameters().Length == 0)
        {
          customType.GetValue(obj).TrimSpaces(modelType);
        }
      }

      return obj;
    }
  }
}
