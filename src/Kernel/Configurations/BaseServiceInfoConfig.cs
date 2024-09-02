namespace LT.DigitalOffice.Kernel.Configurations;

public class BaseServiceInfoConfig
{
  public const string SectionName = "ServiceInfo";

  public string Id { get; init; }
  public string Name { get; init; }
  public string Environment { get; set; }
}
