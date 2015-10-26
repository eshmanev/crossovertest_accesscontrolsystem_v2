using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AccessControl.Web.Models.Account
{
    public class ApplicationUser : IUser<string>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(IIdentity identity)
        {
            var claimIdentity = identity as ClaimsIdentity;
            if (claimIdentity == null)
                return;

            DisplayName = claimIdentity.FindFirstValue("DisplayName");
            Department = claimIdentity.FindFirstValue("Department");
            Site = claimIdentity.FindFirstValue("Site");
            Email = claimIdentity.FindFirstValue("Email");
            PhoneNumber = claimIdentity.FindFirstValue("PhoneNumber");
        }

        public string Id => UserName;

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Site { get; set; }

        public string Department { get; set; }

        public string DisplayName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim("DisplayName", DisplayName));
            userIdentity.AddClaim(new Claim("Department", Department));
            userIdentity.AddClaim(new Claim("Site", Site));
            userIdentity.AddClaim(new Claim("Email", Email));
            userIdentity.AddClaim(new Claim("PhoneNumber", PhoneNumber));

            return userIdentity;
        }
    }
}