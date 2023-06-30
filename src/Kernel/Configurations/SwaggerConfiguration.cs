using System;

namespace DigitalOffice.Kernel.Configurations;

public class SwaggerConfiguration
{
  public const string SectionName = "Swagger";

  private string servicePath = Environment.GetEnvironmentVariable("Service_Path");
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
