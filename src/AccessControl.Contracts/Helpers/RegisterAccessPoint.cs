using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class RegisterAccessPoint : IRegisterAccessPoint
    {
        public RegisterAccessPoint(IAccessPoint accessPoint)
        {
            Contract.Requires(accessPoint != null);
            AccessPoint = accessPoint;
        }

        public IAccessPoint AccessPoint { get; }
    }
}