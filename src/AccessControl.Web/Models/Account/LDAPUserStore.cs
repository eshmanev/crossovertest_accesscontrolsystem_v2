using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using MassTransit;
using Microsoft.AspNet.Identity;

namespace AccessControl.Web.Models.Account
{
    public class LdapUserStore : IUserStore<ApplicationUser>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUser;
        private readonly IRequestClient<IAuthenticateUser, IAuthenticateUserResult> _getPasswordHash;

        public LdapUserStore(IRequestClient<IFindUserByName, IFindUserByNameResult> findUser,
            IRequestClient<IAuthenticateUser, IAuthenticateUserResult> getPasswordHash)
        {
            Contract.Requires(findUser != null);
            Contract.Requires(getPasswordHash != null);

            _findUser = findUser;
            _getPasswordHash = getPasswordHash;
        }

        public void Dispose()
        {
        }

        #region IUserStore<ApplicationUser>

        public Task CreateAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            // userId and userName are equal
            return FindByNameAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var result = await _findUser.Request(new FindUserByName(userName));
            return new ApplicationUser
            {
                UserName = result.UserName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber
            };
        }

        #endregion
    }
}