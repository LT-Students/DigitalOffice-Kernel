using System;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests
{
    /// <summary>
    /// Message request model that is sent to CheckRightsService via MassTransit.
    /// </summary>
    public interface IAccessValidatorCheckRightsServiceRequest
    {
        /// <summary>
        /// User ID.
        /// </summary>
        Guid UserId { get; set; }
        /// <summary>
        /// Right ID.
        /// </summary>
        int RightId { get; set; }

        /// <summary>
        /// Create anonymouse object that can be deserialized into <see cref="IAccessValidatorCheckRightsServiceRequest"/>.
        /// </summary>
        static object CreateObj(Guid userId, int rightId)
        {
            return new
            {
                UserId = userId,
                RightId = rightId
            };
        }
    }
}
