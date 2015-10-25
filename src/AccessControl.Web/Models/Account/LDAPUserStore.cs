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
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;

        public LdapUserStore(IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest)
        {
            Contract.Requires(findUserRequest != null);
            _findUserRequest = findUserRequest;
        }

        public void Dispose()
        {
        }

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
            throw new System.NotImplementedException();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var result = await _findUserRequest.Request(new FindUserByName(userName));
            return new ApplicationUser
            {
                UserName = result.UserName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber
            };
        }
    }
}