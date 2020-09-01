using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.AccessValidator.Interfaces
{
    /// <summary>
    /// Provides access validation methods to check whether user is an administrator
    /// or has certain rights (e.g. to edit or add a new project).
    /// </summary>
    public interface IAccessValidator
    {
        /// <summary>
        /// Checks whether the user is admin or not.
        /// </summary>
        /// <returns>True, if current user has IsAdmin property set to true in the database. False otherwise.</returns>
        Task<bool> IsAdmin();
        /// <summary>
        /// Checks whether the user has certain rights.
        /// </summary>
        /// <param name="rightId">ID of the right.</param>
        /// <returns>True, if there's a UserId-RightId pair in the database. False otherwise.</returns>
        Task<bool> HasRights(int rightId);
    }
}