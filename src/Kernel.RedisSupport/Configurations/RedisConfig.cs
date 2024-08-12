namespace LT.DigitalOffice.Kernel.RedisSupport.Configurations;

/// <summary>
/// Helper class for storing and applying configuration.
/// </summary>
public class RedisConfig
{
  public const string SectionName = "Redis";

  public double CacheLiveInMinutes { get; set; }
}
