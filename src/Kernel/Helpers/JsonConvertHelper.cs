using System;
using System.Text.Json;

namespace LT.DigitalOffice.Kernel.Helpers;

/// <summary>
/// This class allows not using try-catch on null values.
/// </summary>
public static class JsonConvertHelper
{
  private static readonly JsonSerializerOptions _defaultOptions = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
  };

  // Serialize an object to JSON string
  public static string Serialize(object obj, JsonSerializerOptions options = null)
  {
    try
    {
      return JsonSerializer.Serialize(obj, options ?? _defaultOptions);
    }
    catch
    {
      return null;
    }
  }

  // Deserialize a JSON string to an object of type T
  public static T Deserialize<T>(string json, JsonSerializerOptions options = null)
  {
    try
    {
      return JsonSerializer.Deserialize<T>(json, options ?? _defaultOptions);
    }
    catch
    {
      return default;
    }
  }

  // Deserialize with non-generic overload
  public static object Deserialize(string json, Type type, JsonSerializerOptions options = null)
  {
    try
    {
      return JsonSerializer.Deserialize(json, type, options ?? _defaultOptions);
    }
    catch
    {
      return null;
    }
  }
}