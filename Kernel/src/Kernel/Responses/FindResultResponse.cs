using LT.DigitalOffice.Kernel.Enums;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Responses
{
    public class FindResultResponse<T>
    {
        public List<T> Body { get; set; }
        public int TotalCount { get; set; }
        public OperationResultStatusType Status { get; set; }
        public List<string> Errors { get; set; } = new();

        public FindResultResponse(
            List<T> body = default,
            int totalCount = default,
            OperationResultStatusType status = default,
            List<string> errors = default)
        {
            Body = body;
            TotalCount = totalCount;
            Status = status;
            Errors = errors ?? new();
        }
    }
}
