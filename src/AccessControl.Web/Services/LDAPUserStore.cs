using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using AccessControl.Web.Models.Account;
using log4net;
using MassTransit;
using Microsoft.AspNet.Identity;
using IUser = AccessControl.Contracts.IUser;

namespace AccessControl.Web.Services
{
    public class LdapUserStore : IUserStore<ApplicationUser>
    {
        private readonly IRequestClient<IFindUserByName, IUser> _findUser;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LdapUserStore));

        public LdapUserStore(IRequestClient<IFindUserByName, IUser> findUser)
        {
            Contract.Requires(findUser != null);
            _findUser = findUser;
        }

        public void Dispose()
        {
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotSupportedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotSupportedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotSupportedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            // userId and userName are equal
            return FindByNameAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            try
            {
                var result = await _findUser.Request(new FindUserByName(userName));
                return new ApplicationUser
                {
                    UserName = result.UserName,
                    DisplayName = result.DisplayName,
                    Email = result.Email,
                    PhoneNumber = result.PhoneNumber,
                    Site = result.Site,
                    Department = result.Department
                };
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while searching a user", e);
                return null;
            }
            
        }
    }
}