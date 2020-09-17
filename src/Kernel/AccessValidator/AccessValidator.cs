using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidator.Requests;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LT.DigitalOffice.Kernel.AccessValidator
{
    public class AccessValidator : IAccessValidator
    {
        private Guid userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRequestClient<IAccessValidatorCRServiceRequest> requestClientCRS;
        private readonly IRequestClient<IAccessValidatorUserServiceRequest> requestClientUS;

        public AccessValidator(
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] IRequestClient<IAccessValidatorCRServiceRequest> requestClientCRS,
            [FromServices] IRequestClient<IAccessValidatorUserServiceRequest> requestClientUS)
        {
            this.requestClientCRS = requestClientCRS;
            this.requestClientUS = requestClientUS;
            this.httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            if (httpContextAccessor.HttpContext.Request.Headers["UserId"].Count == 0)
            {
                throw new NullReferenceException("There are no UserId headers in the request.");
            }

            var headersUserId = httpContextAccessor.HttpContext.Request.Headers["UserId"];

            if (headersUserId.Count > 1)
            {
                throw new HttpListenerException(400, "There can't be more than one UserId in the request.");
            }

            if (!Guid.TryParse(headersUserId, out Guid userIdFromHeaders))
            {
                throw new FormatException("Incorrect GUID format.");
            }

            return userIdFromHeaders;
        }

        public bool HasRights(int rightId)
        {
            userId = GetCurrentUserId();

            var result = requestClientCRS.GetResponse<IOperationResult<bool>>(new
            {
                UserId = userId,
                RightId = rightId
            }).Result;

            if (result.Message == null)
            {
                throw new Exception("Failed to send request via the broker");
            }

            return result.Message.Body;
        }

        public bool IsAdmin()
        {
            userId = GetCurrentUserId();

            var result = requestClientUS.GetResponse<IOperationResult<bool>>(new
            {
                UserId = userId
            }).Result;

            if (result.Message == null)
            {
                throw new Exception("Failed to send request via the broker");
            }

            return result.Message.Body;
        }
    }
}