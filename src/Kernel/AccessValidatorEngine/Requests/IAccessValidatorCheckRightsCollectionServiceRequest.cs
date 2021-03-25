using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests
{
    public interface IAccessValidatorCheckRightsCollectionServiceRequest
    {
        /// <summary>
        /// User ID.
        /// </summary>
        Guid UserId { get; set; }
        /// <summary>
        /// Right Ids.
        /// </summary>
        IEnumerable<int> RightIds { get; set; }

        /// <summary>
        /// Create anonymouse object that can be deserialized into <see cref="IAccessValidatorCheckRightsCollectionServiceRequest"/>.
        /// </summary>
        static object CreateObj(Guid userId, IEnumerable<int> rightIds)
        {
            return new
            {
                UserId = userId,
                RightIds = rightIds
            };
        }
    }
}
