using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Vendor.API.CodeContracts;

namespace Vendor.API
{
    [ContractClass(typeof(IAccessPointRegistryContract))]
    [ServiceContract]
    public interface IAccessPointRegistry
    {
        /// <summary>
        /// Register a new access point.
        /// </summary>
        /// <param name="dto">Data transfer object.</param>
        [OperationContract]
        void RegisterAccessPoint(AccessPointDto dto);

        /// <summary>
        /// Unregisters an access point which has the specified identifier.
        /// </summary>
        /// <param name="accessPointId">Identifier of the access point.</param>
        [OperationContract]
        void UnregisterAccessPoint(Guid accessPointId);
    }
}