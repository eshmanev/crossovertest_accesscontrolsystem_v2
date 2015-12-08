using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Client.Data;

namespace AccessControl.Client.Consumers
{
    internal class UpdateUserHashVisitor : IAccessPermissionVisitor
    {
        private readonly IAccessPermissionCollection _accessPermissions;
        private readonly string _userName;
        private readonly string _oldHash;
        private readonly string _newHash;

        public UpdateUserHashVisitor(IAccessPermissionCollection accessPermissions, string userName, string oldHash, string newHash)
        {
            Contract.Requires(accessPermissions != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            _accessPermissions = accessPermissions;
            _userName = userName;
            _oldHash = oldHash;
            _newHash = newHash;
        }

        public void Visit(PermanentUserAccess permission)
        {
            if (!string.Equals(permission.UserHash.UserName, _userName))
            {
                return;
            }

            var replacement = new PermanentUserAccess(permission.AccessPointId, new UserHash(permission.UserHash.UserName, _newHash));
            _accessPermissions.AddOrUpdatePermission(replacement);
        }

        public void Visit(PermanentGroupAccess permission)
        {
            var hashes = permission.UserHashes.ToList();
            var index = hashes.FindIndex(x => string.Equals(x.UserName, _userName));
            if (index == -1)
            {
                return;
            }

            var old = hashes[index];
            hashes[index] = new UserHash(old.UserName, _newHash);
            var replacement = new PermanentGroupAccess(permission.AccessPointId, permission.UserGroupName, hashes.ToArray());
            _accessPermissions.AddOrUpdatePermission(replacement);
        }

        public void Visit(ScheduledUserAccess permission)
        {
            if (!string.Equals(permission.UserHash.UserName, _userName))
            {
                return;
            }

            var replacement = new ScheduledUserAccess(
                permission.AccessPointId,
                new UserHash(permission.UserHash.UserName, _newHash),
                permission.WeeklySchedule);

            _accessPermissions.AddOrUpdatePermission(replacement);
        }

        public void Visit(ScheduledGroupAccess permission)
        {
            var hashes = permission.UserHashes.ToList();
            var index = hashes.FindIndex(x => string.Equals(x.UserName, _userName));
            if (index == -1)
            {
                return;
            }

            var old = hashes[index];
            hashes[index] = new UserHash(old.UserName, _newHash);
            var replacement = new ScheduledGroupAccess(permission.AccessPointId, permission.UserGroupName, hashes.ToArray(), permission.WeeklySchedule);
            _accessPermissions.AddOrUpdatePermission(replacement);
        }
    }
}