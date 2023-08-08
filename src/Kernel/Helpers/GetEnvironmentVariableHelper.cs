using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;

namespace LT.DigitalOffice.Kernel.Helpers;

public static class GetEnvironmentVariableHelper
{
  public static string Get(string key)
  {
    return Environment.GetEnvironmentVariable(key)
      ?? throw new InternalServerException($"No environment variable named {key}");
  }
}
