using System;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests
{
    /// <summary>
    /// Message request model that is sent to UserService via MassTransit.
    /// </summary>
    public interface ICheckUserIsAdminRequest
    {
        /// <summary>
        /// User ID.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Create anonymouse object that can be deserialized into <see cref="ICheckUserIsAdminRequest"/>.
        /// </summary>
        static object CreateObj(Guid userId)
        {
            return new
            {
                UserId = userId
            };
        }
    }
}
