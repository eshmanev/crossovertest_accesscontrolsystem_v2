using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.Delegate
{
    public class UserViewModel
    {
        public IUser User { get; set; }
        public bool IsDelegated { get; set; }
    }
}