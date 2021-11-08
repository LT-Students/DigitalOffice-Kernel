namespace LT.DigitalOffice.Kernel.Redis.Configurations
{
  public class RedisConfig
  {
    public const string SectionName = "Redis";

    public double CacheLiveInMinutes { get; set; }
  }
}
