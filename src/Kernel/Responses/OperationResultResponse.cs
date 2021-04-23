using LT.DigitalOffice.Kernel.Enums;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Responses
{
    public class OperationResultResponse<T>
    {
        public T Body { get; set; }
        public OperationResultStatusType Status { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
