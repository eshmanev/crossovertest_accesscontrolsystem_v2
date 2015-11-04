using System.Collections.Generic;
using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.AccessPoint
{
    public class AccessPointIndexViewModel
    {
        public IAccessPoint[] AccessPoints { get; set; } 
        public EditAccessPointViewModel Editor { get; set; }
    }
}