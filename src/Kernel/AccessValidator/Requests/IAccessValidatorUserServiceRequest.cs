using System;

namespace LT.DigitalOffice.Kernel.AccessValidator.Requests
{
    /// <summary>
    /// Message request model that is sent to UserService via MassTransit.
    /// </summary>
    public interface IAccessValidatorUserServiceRequest
    {
        Guid UserId { get; }
        int RightId { get; }
    }
}
