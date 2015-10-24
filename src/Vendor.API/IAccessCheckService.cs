using System.Diagnostics.Contracts;
using System.ServiceModel;
using Vendor.API.CodeContracts;

namespace Vendor.API
{
    [ContractClass(typeof(AccessCheckServiceContract))]
    [ServiceContract]
    public interface IAccessCheckService
    {
        /// <summary>
        /// Check is the user can access the access point.
        /// </summary>
        /// <param name="dto">DTO which contains necessary data.</param>
        /// <returns>true if the user can access the access point; otherwise, false.</returns>
        [OperationContract]
        bool TryPass(AccessCheckDto dto);
    }
}