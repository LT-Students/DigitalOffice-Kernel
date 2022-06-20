using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.Kernel.Helpers
{
  public static class ResponseCreator
  {
    private const string Forbidden = "Not enough rights.";
    private const string BadRequest = "Request is not correct.";
    private const string NotFound = "Nothing found on request.";

    private static IHttpContextAccessor _httpContextAccessor;

    public static void ResponseCreatorConfigure(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public static OperationResultResponse<T> CreateSuccessResponse<T>(HttpStatusCode statusCode, T body, List<string> errors = null)
    {
      if (_httpContextAccessor is not null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)statusCode;
      }

      return new OperationResultResponse<T>
      {
        Body = body,
        Errors = errors
      };
    }

    public static OperationResultResponse<T> CreateFailureResponse<T>(HttpStatusCode statusCode, List<string> errors = null)
    {
      if (_httpContextAccessor is not null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)statusCode;
      }

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

    public static FindResultResponse<T> CreateFailureFindResponse<T>(HttpStatusCode statusCode, List<string> errors = null)
    {
      if (_httpContextAccessor is not null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)statusCode;
      }

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
