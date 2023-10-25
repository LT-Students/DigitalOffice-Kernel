using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LTDO.Kernel.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum OperationResultStatusType
{
  FullSuccess,
  PartialSuccess,
  Failed
}
