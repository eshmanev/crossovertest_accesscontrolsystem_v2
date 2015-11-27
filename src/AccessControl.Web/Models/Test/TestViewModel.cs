using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.Test
{
    public class TestViewModel
    {
        public IUserBiometric[] Biometrics { get; set; }
        public IAccessPoint[] AccessPoints { get; set; }
    }
}