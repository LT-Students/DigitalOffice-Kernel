using System;

namespace LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests
{
  /// <summary>
  /// Message request model that is sent to UserService via MassTransit.
  /// </summary>
  public interface ICheckUserAnyRightRequest
  {
    /// <summary>
    /// User ID.
    /// </summary>
    Guid UserId { get; }

    int[] RightIds { get; }

    /// <summary>
    /// Create anonymouse object that can be deserialized into <see cref="ICheckUserAnyRightRequest"/>.
    /// </summary>
    static object CreateObj(Guid userId, params int[] rightIds)
    {
      return new
      {
        UserId = userId,
        RightIds = rightIds
      };
    }
  }
}
