using System;

namespace LTDO.Kernel.Configurations;

public class SwaggerConfiguration
{
  private string servicePath = Environment.GetEnvironmentVariable("Service_Path");

  public const string SectionName = "Swagger";

  public string ServicePath
  {
    get
    {
      return servicePath;
    }
    init
    {
      if (servicePath is null)
      {
        servicePath = value ?? string.Empty;
      }
    }
  }
}
