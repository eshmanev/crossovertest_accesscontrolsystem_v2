using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class RegisterAccessPoint : IRegisterAccessPoint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterAccessPoint" /> class.
        /// </summary>
        /// <param name="accessPoint">The access point.</param>
        public RegisterAccessPoint(IAccessPoint accessPoint)
        {
            Contract.Requires(accessPoint != null);
            AccessPoint = accessPoint;
        }

        /// <summary>
        ///     Gets the access point.
        /// </summary>
        /// <value>
        ///     The access point.
        /// </value>
        public IAccessPoint AccessPoint { get; }
    }
}