using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AccessControl.Client.Data;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Helpers;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Services
{
    internal class AccessPermissionService : IAccessPermissionService
    {
        private readonly IRequestClient<IListAccessRights, IListAccessRightsResult> _listAccessRightsRequest;
        private readonly IRequestClient<IListUsersBiometric, IListUsersBiometricResult> _listUsersBiometricRequest;
        private const string StorageFileName = "permissions.dat";

        public AccessPermissionService(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
                                       IRequestClient<IListUsersBiometric, IListUsersBiometricResult> listUsersBiometricRequest)
        {
            Contract.Requires(listAccessRightsRequest != null);
            Contract.Requires(listUsersBiometricRequest != null);

            _listAccessRightsRequest = listAccessRightsRequest;
            _listUsersBiometricRequest = listUsersBiometricRequest;
        }

        public void Load(IAccessPermissionCollection accessPermissions)
        {
            if (!File.Exists(StorageFileName))
                return;

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(StorageFileName, FileMode.Open))
            {
                var deserialized = formatter.Deserialize(stream) as IAccessPermissionCollection;
                deserialized.ForEach(accessPermissions.AddOrUpdatePermission);
            }
        }

        public void Save(IAccessPermissionCollection accessPermissions)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(StorageFileName, FileMode.Create))
            {
                formatter.Serialize(stream , accessPermissions);
            }
        }

        public async void Update(IAccessPermissionCollection accessPermissions)
        {
            var biometricInfo = await _listUsersBiometricRequest.Request(ListCommand.Default);
            var biometricInfoMap = biometricInfo.Users.ToDictionary(x => x.UserName);

            var accessRights = await _listAccessRightsRequest.Request(ListCommand.Default);
            foreach (var userAccessRights in accessRights.UserAccessRights)
            {
                var userName = userAccessRights.UserName;
                var userHash = biometricInfoMap.ContainsKey(userName) ? biometricInfoMap[userName].BiometricHash : null;

                userAccessRights.PermanentAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new PermanentUserAccess(rule.AccessPointId, new UserHash(userName, userHash));
                        accessPermissions.AddOrUpdatePermission(permission);
                    });

                userAccessRights.ScheduledAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new ScheduledUserAccess(rule.AccessPointId, new UserHash(userName, userHash), rule.FromTimeUtc, rule.ToTimeUtc);
                        accessPermissions.AddOrUpdatePermission(permission);
                    });
            }

            foreach (var groupRights in accessRights.UserGroupAccessRights)
            {
                
            }
        }
    }
}