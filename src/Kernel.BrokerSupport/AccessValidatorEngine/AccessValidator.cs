using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine
{
  /// <inheritdoc/>
  public class AccessValidator : IAccessValidator
  {
    private readonly HttpContext _httpContext;
    private readonly ILogger<AccessValidator> _logger;
    private readonly IRequestClient<ICheckUserRightsRequest> _requestClientCR;
    private readonly IRequestClient<ICheckUserIsAdminRequest> _requestClientUS;

    private async Task<bool> IsUserAdminAsync(Guid userId)
    {
      Response<IOperationResult<bool>> result =
        await _requestClientUS.GetResponse<IOperationResult<bool>>(
          ICheckUserIsAdminRequest.CreateObj(userId),
          timeout: RequestTimeout.After(s: 2));

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
    public async Task<bool> HasRightsAsync(params int[] rightIds)
    {
      return await HasRightsAsync(null, true, rightIds);
    }

    /// <inheritdoc/>
    public async Task<bool> HasRightsAsync(Guid? userId, params int[] rightIds)
    {
      return await HasRightsAsync(userId, true, rightIds);
    }

    public async Task<bool> HasRightsAsync(Guid? userId, bool includeIsAdminCheck, params int[] rightIds)
    {
      if (rightIds == null || !rightIds.Any())
      {
        throw new ArgumentException("Can not check empty rights array.", nameof(rightIds));
      }

      if (!userId.HasValue)
      {
        userId = _httpContext.GetUserId();
      }

      if (includeIsAdminCheck && await IsUserAdminAsync(userId.Value))
      {
        return true;
      }

      var result = _requestClientCR.GetResponse<IOperationResult<bool>>(
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
    public async Task<bool> IsAdminAsync(Guid? userId = null)
    {
      if (!userId.HasValue)
      {
        userId = _httpContext.GetUserId();
      }

      return await IsUserAdminAsync(userId.Value);
    }
  }
}
