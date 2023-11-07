using DigitalOffice.Kernel.Attributes;
using DigitalOffice.Kernel.Responses;
using System.Collections.Generic;
using System.Net;

namespace DigitalOffice.Kernel.Helpers.Interfaces;

[AutoInject]
public interface IResponseCreator
{
  OperationResultResponse<T> CreateFailureResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
  FindResultResponse<T> CreateFailureFindResponse<T>(HttpStatusCode statusCode, List<string> errors = null);
}
