using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine
{
    /// <inheritdoc/>
    public class AccessValidator : IAccessValidator
    {
        private readonly HttpContext _httpContext;
        private readonly IRequestClient<ICheckUserRightsRequest> _requestClientCR;
        private readonly IRequestClient<ICheckUserIsAdminRequest> _requestClientUS;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccessValidator(
            IHttpContextAccessor httpContextAccessor,
            IRequestClient<ICheckUserRightsRequest> requestClientCR,
            IRequestClient<ICheckUserIsAdminRequest> requestClientUS)
        {
            _requestClientCR = requestClientCR;
            _requestClientUS = requestClientUS;
            _httpContext = httpContextAccessor.HttpContext;
        }

        private bool CheckUserRights(Guid? userId, int[] rightIds)
        {
            if (rightIds == null || !rightIds.Any())
            {
                throw new ArgumentException("Can not check empty rights array.", nameof(rightIds));
            }

            if (userId == null)
            {
                userId = _httpContext.GetUserId();
            }
            else
            {
                userId = (Guid)userId;
            }

            Response<IOperationResult<bool>> result = _requestClientCR.GetResponse<IOperationResult<bool>>(
                ICheckUserRightsRequest.CreateObj((Guid)userId, rightIds),
                timeout: RequestTimeout.After(s: 2)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request to CheckRightService via the broker.");
            }

            return result.Message.Body;
        }

        /// <inheritdoc/>
        public bool HasRights(params int[] rightIds)
        {
            return CheckUserRights(null, rightIds);
        }

        /// <inheritdoc/>
        public bool HasRights(Guid? userId, params int[] rightIds)
        {
            return CheckUserRights(userId, rightIds);
        }

        /// <inheritdoc/>
        public bool IsAdmin(Guid? userId = null)
        {
            if (userId == null)
            {
                userId = _httpContext.GetUserId();
            }
            else
            {
                userId = (Guid)userId;
            }

            Response<IOperationResult<bool>> result = _requestClientUS.GetResponse<IOperationResult<bool>>(
                ICheckUserIsAdminRequest.CreateObj((Guid)userId),
                timeout: RequestTimeout.After(s: 2)).Result;

            if (result.Message == null)
            {
                throw new NullReferenceException("Failed to send request to UserService via the broker.");
            }

            return result.Message.Body;
        }
    }
}