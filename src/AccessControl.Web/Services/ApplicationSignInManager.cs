using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Models.Account;
using log4net;
using MassTransit;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AccessControl.Web.Services
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        private readonly IRequestClient<IAuthenticateUser, IAuthenticateUserResult> _authenticateRequest;
        private static ILog Log = LogManager.GetLogger(typeof(ApplicationSignInManager));

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, IRequestClient<IAuthenticateUser, IAuthenticateUserResult> authenticateRequest)
            : base(userManager, authenticationManager)
        {
            _authenticateRequest = authenticateRequest;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context, IRequestClient<IAuthenticateUser, IAuthenticateUserResult> authenticateRequest)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication, authenticateRequest);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (this.UserManager == null)
                return SignInStatus.Failure;

            var user = await base.UserManager.FindByNameAsync(userName);
            if (user == null)
                return SignInStatus.Failure;

            var message = new AuthenticateUser(user.Id, password);
            try
            {
                var result = await _authenticateRequest.Request(message);
                if (!result.Authenticated)
                    return SignInStatus.Failure;
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while authentication", e);
                return SignInStatus.Failure;
            }
            

            await SignInAsync(user, isPersistent, false);
            return SignInStatus.Success;
        }
    }
}