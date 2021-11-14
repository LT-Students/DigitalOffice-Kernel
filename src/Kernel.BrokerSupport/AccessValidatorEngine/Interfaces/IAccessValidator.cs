﻿using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces
{
  /// <summary>
  /// Provides access validation methods to check whether user is an administrator
  /// or has certain rights (e.g. to edit or add a new project).
  /// </summary>
  [AutoInject]
  public interface IAccessValidator
  {
    /// <summary>
    /// Checks whether the user is admin or not.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>True, if current user has IsAdmin property set to true in the database. False otherwise.</returns>
    Task<bool> IsAdminAsync(Guid? userId = null);

    /// <summary>
    /// Checks whether the user has certain rights.
    /// </summary>
    /// <param name="rightIds">Ids of the rigths.</param>
    /// <returns>True, if there's a UserId-RightId pair for all requsted rights in the database. False otherwise.</returns>
    Task<bool> HasRightsAsync(params int[] rightIds);

    /// <summary>
    /// Checks whether the user has certain rights.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <param name="rightIds">Ids of the rigths.</param>
    /// <returns>True, if there's a UserId-RightId pair for all requsted rights in the database. False otherwise.</returns>
    Task<bool> HasRightsAsync(Guid? userId, params int[] rightIds);

    /// <summary>
    /// Checks whether the user has certain rights.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <param name="includeIsAdminCheck">If this is true, the method should be included in the check whether the user is an admin.</param>
    /// <param name="rightIds">Ids of the rigths.</param>
    /// <returns>True, if there's a UserId-RightId pair for all requsted rights in the database. False otherwise.</returns>
    Task<bool> HasRightsAsync(Guid? userId, bool includeIsAdminCheck, params int[] rightIds);
  }
}
