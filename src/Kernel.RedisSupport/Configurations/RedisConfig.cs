namespace LT.DigitalOffice.Kernel.RedisSupport.Configurations;

/// <summary>
/// Helper class for storing and applying configuration.
/// </summary>
public class RedisConfig
{
  /// <summary>
  /// Config section name.
  /// </summary>
  public const string SectionName = "Redis";

  /// <summary>
  /// How long values must be cached.
  /// </summary>
  public double CacheLiveInMinutes { get; set; }
}
