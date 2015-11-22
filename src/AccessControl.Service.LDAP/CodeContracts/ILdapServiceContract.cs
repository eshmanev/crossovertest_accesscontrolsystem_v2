using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Service.LDAP.Services;

namespace AccessControl.Service.LDAP.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ILdapService" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ILdapService))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ILdapServiceContract : ILdapService
    {
        public bool Authenticate(string userName, string password)
        {
            return false;
        }

        public IEnumerable<IDepartment> FindDepartmentsByManager(string managerName)
        {
            Contract.Requires(managerName != null);
            return null;
        }

        public IUser FindUserByName(string userName)
        {
            Contract.Requires(userName != null);
            return null;
        }

        public IEnumerable<IUser> FindUsersByManager(string managerName)
        {
            Contract.Requires(managerName != null);
            return null;
        }

        public bool ValidateDepartment(string site, string department)
        {
            Contract.Requires(site != null);
            Contract.Requires(department != null);
            return false;
        }

        public IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName)
        {
            Contract.Requires(managerName != null);
            return null;
        }
    }
}