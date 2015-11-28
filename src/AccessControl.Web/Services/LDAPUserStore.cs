using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Web.Models.Account;
using log4net;
using MassTransit;
using Microsoft.AspNet.Identity;

namespace AccessControl.Web.Services
{
    public class LdapUserStore : IUserStore<ApplicationUser>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUser;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LdapUserStore));

        public LdapUserStore(IRequestClient<IFindUserByName, IFindUserByNameResult> findUser)
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
                return result.User == null
                           ? null
                           : new ApplicationUser
                           {
                               UserName = result.User.UserName,
                               DisplayName = result.User.DisplayName,
                               Email = result.User.Email,
                               PhoneNumber = result.User.PhoneNumber,
                               Site = result.User.Site,
                               Department = result.User.Department
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