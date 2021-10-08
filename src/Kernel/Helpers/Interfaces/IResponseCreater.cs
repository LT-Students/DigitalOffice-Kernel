using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System.Collections.Generic;
using System.Net;

namespace LT.DigitalOffice.Kernel.Helpers.Interfaces
{
    [AutoInject]
    public interface IResponseCreater
    {
        OperationResultResponse<T> CreateFailureResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
        FindResultResponse<T> CreateFailureFindResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
    }
}
