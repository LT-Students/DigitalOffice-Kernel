using System.Text.Json.Serialization;

namespace LT.DigitalOffice.Kernel.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<OperationResultStatusType>))]
public enum OperationResultStatusType
{
  FullSuccess,
  PartialSuccess,
  Failed
}
