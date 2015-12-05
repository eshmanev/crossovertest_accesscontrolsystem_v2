using System;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.AccessRights
{
    public class AccessRightsIndexViewModel
    {
        public IUserAccessRights[] UserAccessRights { get; set; }

        public IUserGroupAccessRights[] UserGroupAccessRights { get; set; }

        public EditAccessRightsViewModel Editor { get; set; }

        public string GetDisplayUserName(string userName)
        {
            var user = Editor.Users.FirstOrDefault(x => x.UserName == userName);
            return user != null ? user.DisplayName : userName;
        }

        public string GetDisplayGroupName(string groupName)
        {
            var group = Editor.UserGroups.FirstOrDefault(x => x.Name == groupName);
            return group != null ? group.DisplayName : groupName;
        }

        public string GetDepartment(Guid accessPointId)
        {
            var accessPoint = Editor.AccessPoints.FirstOrDefault(x => x.AccessPointId == accessPointId);
            return accessPoint != null ? accessPoint.Department : "Unknown department";
        }

        public string GetName(Guid accessPointId)
        {
            var accessPoint = Editor.AccessPoints.FirstOrDefault(x => x.AccessPointId == accessPointId);
            return accessPoint != null ? accessPoint.Name : "Unknown access point";
        }
    }
}