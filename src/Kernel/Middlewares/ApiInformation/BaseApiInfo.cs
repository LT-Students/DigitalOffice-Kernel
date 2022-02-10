using System;
using LT.DigitalOffice.Kernel.Extensions;

namespace LT.DigitalOffice.Kernel.Middlewares.ApiInformation
{
  public abstract class BaseApiInfo
  {
    public static string Version { get; protected set; }
    public static DateTime StartTime { get; protected set; }
    public static string ApiName { get; protected set; }
    public static string Description { get; protected set; }

    public static object GetResponse()
    {
      return new
      {
        ApiName,
        Version,
        Description,
        UpTime = StartTime.ToServiceUpTime()
      };
    }
  }
}
