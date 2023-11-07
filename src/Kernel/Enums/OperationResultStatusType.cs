using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.Kernel.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum OperationResultStatusType
{
  FullSuccess,
  PartialSuccess,
  Failed
}
