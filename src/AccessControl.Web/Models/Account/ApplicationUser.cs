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

            UserName = claimIdentity.FindFirstValue("UserName");
            DisplayName = claimIdentity.FindFirstValue("DisplayName");
            Department = claimIdentity.FindFirstValue("Department");
            Email = claimIdentity.FindFirstValue("Email");
            PhoneNumber = claimIdentity.FindFirstValue("PhoneNumber");
            ServiceTicket = claimIdentity.FindFirstValue("ServiceTicket");
        }

        public ApplicationUser(Contracts.Dto.IUser user)
        {
            UserName = user.UserName;
            DisplayName = user.DisplayName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Department = user.Department;
        }

        public string Id => UserName;

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Department { get; set; }

        public string DisplayName { get; set; }

        public string ServiceTicket { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            userIdentity.AddClaim(new Claim("UserName", UserName));
            if (!string.IsNullOrWhiteSpace(DisplayName))
                userIdentity.AddClaim(new Claim("DisplayName", DisplayName));

            if (!string.IsNullOrWhiteSpace(Department))
                userIdentity.AddClaim(new Claim("Department", Department));

            if (!string.IsNullOrWhiteSpace(Email))
                userIdentity.AddClaim(new Claim("Email", Email));

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
                userIdentity.AddClaim(new Claim("PhoneNumber", PhoneNumber));

            if (!string.IsNullOrWhiteSpace(ServiceTicket))
                userIdentity.AddClaim(new Claim("ServiceTicket", ServiceTicket));

            return userIdentity;
        }
    }
}