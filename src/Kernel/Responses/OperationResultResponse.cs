using LT.DigitalOffice.Kernel.Enums;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Responses
{
    public class OperationResultResponse<T>
    {
        public T Body { get; set; }
        public string Status { get; set; }
        public List<string> Errors { get; set; } = new();

        public OperationResultResponse(
            T body,
            OperationResultStatusType status,
            List<string> errors)
        {
            Body = body;
            Status = status.ToString();
            Errors = errors ?? new();
        }
    }
}
