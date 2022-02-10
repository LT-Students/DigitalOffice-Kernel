using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.Kernel.Helpers.Interfaces
{
  [AutoInject]
  public interface IResponseCreator
  {
    OperationResultResponse<T> CreateFailureResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
    FindResultResponse<T> CreateFailureFindResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
  }
}
