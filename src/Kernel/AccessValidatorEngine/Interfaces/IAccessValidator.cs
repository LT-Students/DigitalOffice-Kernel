﻿using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces
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
        /// <returns>True, if current user has IsAdmin property set to true in the database. False otherwise.</returns>
        bool IsAdmin();

        /// <summary>
        /// Checks whether the user has certain rights.
        /// </summary>
        /// <param name="rightIds">Ids of the rigths.</param>
        /// <returns>True, if there's a UserId-RightId pair for all requsted rights in the database. False otherwise.</returns>
        bool HasRights(params int[] rightIds);
    }
}