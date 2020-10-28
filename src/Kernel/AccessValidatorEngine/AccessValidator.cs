using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine
{
    /// <inheritdoc/>
    public class AccessValidator : IAccessValidator
    {
        private Guid userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRequestClient<IAccessValidatorCheckRightsServiceRequest> requestClientCRS;
        private readonly IRequestClient<IAccessValidatorUserServiceRequest> requestClientUS;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccessValidator(
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] IRequestClient<IAccessValidatorCheckRightsServiceRequest> requestClientCRS,
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
                throw new BadRequestException("There are no UserId headers in the request.");
            }

            var headersUserId = httpContextAccessor.HttpContext.Request.Headers["UserId"];

            if (headersUserId.Count > 1)
            {
                throw new BadRequestException("There can't be more than one UserId in the request.");
            }

            if (!Guid.TryParse(headersUserId, out Guid userIdFromHeaders))
            {
                throw new BadRequestException("Incorrect GUID format.");
            }

            return userIdFromHeaders;
        }

        /// <inheritdoc/>
        public bool HasRights(int rightId)
        {
            userId = GetCurrentUserId();

            var result = requestClientCRS.GetResponse<IOperationResult<bool>>(
                IAccessValidatorCheckRightsServiceRequest.CreateObj(userId, rightId)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request via the broker.");
            }

            return result.Message.Body;
        }

        /// <inheritdoc/>
        public bool IsAdmin()
        {
            userId = GetCurrentUserId();

            var result = requestClientUS.GetResponse<IOperationResult<bool>>(
                IAccessValidatorUserServiceRequest.CreateObj(userId)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request via the broker.");
            }

            return result.Message.Body;
        }
    }
}