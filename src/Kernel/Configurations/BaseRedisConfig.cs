namespace LT.DigitalOffice.UserService.Models.Dto.Configurations
{
  public class BaseRedisConfig
  {
    public const string SectionName = "Redis";

    public double CacheLiveInMinutes { get; init; }
  }
}
