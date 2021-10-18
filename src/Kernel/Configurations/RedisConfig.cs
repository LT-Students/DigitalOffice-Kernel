namespace LT.DigitalOffice.UserService.Models.Dto.Configurations
{
  public class RedisConfig
  {
    public const string SectionName = "Redis";

    public double CacheLiveInMinutes { get; set; }
  }
}
