using System;
using LT.DigitalOffice.Kernel.Exceptions.Models;

namespace LT.DigitalOffice.Kernel.Helpers
{
  public static class GetEnvironmentVariableHelper
  {
    public static string Get(string key)
    {
      return Environment.GetEnvironmentVariable(key)
        ?? throw new InternalServerException($"No environment variable named {key}");
    }
  }
}
