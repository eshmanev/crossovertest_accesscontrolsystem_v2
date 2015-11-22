using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Web.Models.Account
{
    public class ApplicationUser : IUser<string>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(Contracts.Dto.IUser user, string[] roles)
        {
            UserName = user.UserName;
            DisplayName = user.DisplayName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Site = user.Site;
            Department = user.Department;
            Roles = roles;
        }

        public ApplicationUser(IIdentity identity)
        {
            var claimIdentity = identity as ClaimsIdentity;
            if (claimIdentity == null)
                return;

            UserName = claimIdentity.FindFirstValue("UserName");
            DisplayName = claimIdentity.FindFirstValue("DisplayName");
            Department = claimIdentity.FindFirstValue("Department");
            Site = claimIdentity.FindFirstValue("Site");
            Email = claimIdentity.FindFirstValue("Email");
            PhoneNumber = claimIdentity.FindFirstValue("PhoneNumber");
            Roles = claimIdentity.FindAll(x => x.Type == "Role").Select(x => x.Value).ToArray();
        }

        public string Id => UserName;

        public string UserName { get; set; }

        public string Email { get; }

        public string PhoneNumber { get; }

        public string Site { get; }

        public string Department { get; }

        public string DisplayName { get; }

        public string[] Roles { get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            userIdentity.AddClaim(new Claim("UserName", UserName));
            if (!string.IsNullOrWhiteSpace(DisplayName))
                userIdentity.AddClaim(new Claim("DisplayName", DisplayName));

            if (!string.IsNullOrWhiteSpace(Department))
                userIdentity.AddClaim(new Claim("Department", Department));

            if (!string.IsNullOrWhiteSpace(Site))
                userIdentity.AddClaim(new Claim("Site", Site));

            if (!string.IsNullOrWhiteSpace(Email))
                userIdentity.AddClaim(new Claim("Email", Email));

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
                userIdentity.AddClaim(new Claim("PhoneNumber", PhoneNumber));

            Roles?.ForEach(x => userIdentity.AddClaim(new Claim("Role", x)));

            return userIdentity;
        }
    }
}