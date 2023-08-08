using System;

namespace DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Requests;

public interface ICheckDepartmentManagerRequest
{
  Guid UserId { get; }

  Guid DepartmentId { get; }

  static object CreateObj(Guid userId, Guid departmentId)
  {
    return new
    {
      UserId = userId,
      DepartmentId = departmentId
    };
  }
}
