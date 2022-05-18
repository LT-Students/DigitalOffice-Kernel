﻿using System;
using LT.DigitalOffice.Kernel.Helpers;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LT.DigitalOffice.Kernel.EFSupport.Helpers
{
  public static class ConnectionStringHandler
  {
    public static string GetConnectionString(IConfiguration configuration)
    {
      string connStr = Environment.GetEnvironmentVariable("ConnectionString");

      if (string.IsNullOrEmpty(connStr))
      {
        connStr = configuration.GetConnectionString("SQLConnectionString");

        Log.Information($"SQL connection string from appsettings.json was used. " +
          $"Value '{PasswordHider.Hide(connStr)}'.");
      }
      else
      {
        Log.Information($"SQL connection string from environment was used. " +
          $"Value '{PasswordHider.Hide(connStr)}'.");
      }
      return connStr;
    }
  }
}
