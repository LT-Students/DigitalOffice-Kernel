using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine
{
    /// <inheritdoc/>
    public class AccessValidator : IAccessValidator
    {
        private readonly HttpContext _httpContext;
        private readonly ILogger<AccessValidator> _logger;
        private readonly IRequestClient<ICheckUserRightsRequest> _requestClientCR;
        private readonly IRequestClient<ICheckUserIsAdminRequest> _requestClientUS;

        private bool IsUserAdmin(Guid userId)
        {
            Response<IOperationResult<bool>> result = _requestClientUS.GetResponse<IOperationResult<bool>>(
                ICheckUserIsAdminRequest.CreateObj(userId),
                timeout: RequestTimeout.After(s: 2)).Result;

            if (result.Message == null)
            {
                _logger.LogWarning("Failed to send request to UserService via the broker.");

                return false;
            }

            return result.Message.Body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccessValidator(
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccessValidator> logger,
            IRequestClient<ICheckUserRightsRequest> requestClientCR,
            IRequestClient<ICheckUserIsAdminRequest> requestClientUS)
        {
            _requestClientCR = requestClientCR;
            _requestClientUS = requestClientUS;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public bool HasRights(params int[] rightIds)
        {
            return HasRights(null, true, rightIds);
        }

        /// <inheritdoc/>
        public bool HasRights(Guid? userId, params int[] rightIds)
        {
            return HasRights(userId, true, rightIds);
        }

        public bool HasRights(Guid? userId, bool includeIsAdminCheck, params int[] rightIds)
        {
            if (rightIds == null || !rightIds.Any())
            {
                throw new ArgumentException("Can not check empty rights array.", nameof(rightIds));
            }

            if (!userId.HasValue)
            {
                userId = _httpContext.GetUserId();
            }

            if (includeIsAdminCheck && IsUserAdmin(userId.Value))
            {
                return true;
            }

            Response<IOperationResult<bool>> result = _requestClientCR.GetResponse<IOperationResult<bool>>(
                ICheckUserRightsRequest.CreateObj(userId.Value, rightIds),
                timeout: RequestTimeout.After(s: 2)).Result;

            if (result.Message == null)
            {
                _logger.LogWarning("Failed to send request to CheckRightService via the broker.");

                return false;
            }

            return result.Message.Body;
        }

        /// <inheritdoc/>
        public bool IsAdmin(Guid? userId = null)
        {
            if (!userId.HasValue)
            {
                userId = _httpContext.GetUserId();
            }

            return IsUserAdmin(userId.Value);
        }
    }
}