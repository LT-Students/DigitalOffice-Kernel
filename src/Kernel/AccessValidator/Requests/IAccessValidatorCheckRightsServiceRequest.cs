﻿using System;

namespace LT.DigitalOffice.Kernel.AccessValidator.Requests
{
    /// <summary>
    /// Message request model that is sent to CheckRightsService via MassTransit.
    /// </summary>
    public interface IAccessValidatorCheckRightsServiceRequest
    {
        Guid UserId { get; set; }
        int RightId { get; set; }
    }
}