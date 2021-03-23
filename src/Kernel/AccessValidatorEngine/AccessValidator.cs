using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine
{
    /// <inheritdoc/>
    public class AccessValidator : IAccessValidator
    {
        private Guid _userId;

        private readonly HttpContext _httpContext;
        private readonly IRequestClient<IAccessValidatorCheckRightsServiceRequest> _requestClientCheckRightService;
        private readonly IRequestClient<IAccessValidatorUserServiceRequest> _requestClientUserService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccessValidator(
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] IRequestClient<IAccessValidatorCheckRightsServiceRequest> requestClientCRS,
            [FromServices] IRequestClient<IAccessValidatorUserServiceRequest> requestClientUS)
        {
            _requestClientCheckRightService = requestClientCRS;
            _requestClientUserService = requestClientUS;
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <inheritdoc/>
        public bool HasRight(int rightId)
        {
            _userId = _httpContext.GetUserId();

            var result = _requestClientCheckRightService.GetResponse<IOperationResult<bool>>(
                IAccessValidatorCheckRightsServiceRequest.CreateObj(_userId, rightId)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request to CheckRightService via the broker.");
            }

            return result.Message.Body;
        }

        public bool HasRights(IEnumerable<int> rightIds)
        {
            _userId = _httpContext.GetUserId();

            var result = _requestClientCheckRightService.GetResponse<IOperationResult<bool>>(
                IAccessValidatorCheckRightsCollectionServiceRequest.CreateObj(_userId, rightIds)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request to CheckRightService via the broker.");
            }

            return result.Message.Body;
        }

        /// <inheritdoc/>
        public bool IsAdmin()
        {
            _userId = _httpContext.GetUserId();

            var result = _requestClientUserService.GetResponse<IOperationResult<bool>>(
                IAccessValidatorUserServiceRequest.CreateObj(_userId)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request to UserService via the broker.");
            }

            return result.Message.Body;
        }
    }
}