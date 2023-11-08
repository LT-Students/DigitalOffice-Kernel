using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using Serilog;
using System;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Helpers;

public class RabbitMqCredentialsHelper
{
  public static (string username, string password) Get(BaseRabbitMqConfig rabbitMqConfig, Kernel.Configurations.BaseServiceInfoConfig serviceInfoConfig)
  {
    static string GetString(string envVar, string fromAppsettings, string generated, string fieldName)
    {
      string str = Environment.GetEnvironmentVariable(envVar);
      if (string.IsNullOrEmpty(str))
      {
        str = fromAppsettings ?? generated;

        Log.Information(
          fromAppsettings == null
            ? $"Default RabbitMq {fieldName} was used."
            : $"RabbitMq {fieldName} from appsetings.json was used.");
      }
      else
      {
        Log.Information($"RabbitMq {fieldName} from environment was used.");
      }

      return str;
    }

    return (GetString("RabbitMqUsername", rabbitMqConfig.Username, $"{serviceInfoConfig.Name}_{serviceInfoConfig.Id}", "Username"),
      GetString("RabbitMqPassword", rabbitMqConfig.Password, serviceInfoConfig.Id, "Password"));
  }
}
