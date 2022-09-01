using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.Kernel.Helpers
{
  public class ResponseCreator : IResponseCreator
  {
    private const string Forbidden = "Not enough rights.";
    private const string BadRequest = "Request is not correct.";
    private const string NotFound = "Nothing found on request.";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResponseCreator(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<T> CreateFailureResponse<T>(HttpStatusCode statusCode, List<string> errors = null)
    {
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)statusCode;

      if (errors == null)
      {
        switch (statusCode)
        {
          case HttpStatusCode.Forbidden:
            errors = new() { Forbidden };
            break;
          case HttpStatusCode.BadRequest:
            errors = new() { BadRequest };
            break;
          case HttpStatusCode.NotFound:
            errors = new() { NotFound };
            break;
        }
      }

      return new OperationResultResponse<T>
      {
        Body = default,
        Errors = errors
      };
    }

    public FindResultResponse<T> CreateFailureFindResponse<T>(HttpStatusCode statusCode, List<string> errors = null)
    {
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)statusCode;

      if (errors == null)
      {
        switch (statusCode)
        {
          case HttpStatusCode.Forbidden:
            errors = new() { Forbidden };
            break;
          case HttpStatusCode.BadRequest:
            errors = new() { BadRequest };
            break;
          case HttpStatusCode.NotFound:
            errors = new() { NotFound };
            break;
        }
      }

      return new FindResultResponse<T>
      {
        TotalCount = 0,
        Body = default,
        Errors = errors
      };
    }
  }
}
