﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using System;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests
{
    /// <summary>
    /// Message request model that is sent to UserService via MassTransit.
    /// </summary>
    public interface ICheckUserRightsRequest
    {
        /// <summary>
        /// User ID.
        /// </summary>
        Guid UserId { get; }

        int[] RightIds { get; }

        /// <summary>
        /// Create anonymouse object that can be deserialized into <see cref="ICheckUserRightsRequest"/>.
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
