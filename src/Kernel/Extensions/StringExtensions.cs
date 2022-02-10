using System;
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
  }
}
