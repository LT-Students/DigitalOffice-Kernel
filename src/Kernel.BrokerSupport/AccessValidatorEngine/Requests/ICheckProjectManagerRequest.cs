using System;

namespace DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests
{
  public interface ICheckProjectManagerRequest
  {
    Guid UserId { get; }

    Guid ProjectId { get; }

    static object CreateObj(Guid userId, Guid projectId)
    {
      return new
      {
        UserId = userId,
        ProjectId = projectId
      };
    }
  }
}
