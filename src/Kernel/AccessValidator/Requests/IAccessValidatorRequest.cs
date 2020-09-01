using System;

namespace LT.DigitalOffice.Kernel.AccessValidator.Requests
{
    /// <summary>
    /// Message request model that is sent to UserService and CheckRightsService via MassTransit.
    /// </summary>
    public interface IAccessValidatorRequest
    {
        Guid UserId { get; set; }
        int RightId { get; set; }
    }
}
