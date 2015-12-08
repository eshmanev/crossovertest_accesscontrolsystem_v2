using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Impl.Commands;
using log4net;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Services
{
    internal class AccessPermissionService : IAccessPermissionService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccessPermissionService));
        private readonly IRequestClient<IListAccessRights, IListAccessRightsResult> _listAccessRightsRequest;
        private readonly IRequestClient<IListUsersBiometric, IListUsersBiometricResult> _listUsersBiometricRequest;
        private readonly IRequestClient<IListUsersInGroup, IListUsersInGroupResult> _listUsersInGroupRequest;
        private const string StorageFileName = "permissions.dat";

        public AccessPermissionService(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
                                       IRequestClient<IListUsersBiometric, IListUsersBiometricResult> listUsersBiometricRequest,
                                       IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest)
        {
            Contract.Requires(listAccessRightsRequest != null);
            Contract.Requires(listUsersBiometricRequest != null);
            Contract.Requires(listUsersInGroupRequest != null);

            _listAccessRightsRequest = listAccessRightsRequest;
            _listUsersBiometricRequest = listUsersBiometricRequest;
            _listUsersInGroupRequest = listUsersInGroupRequest;
        }

        public void Load(IAccessPermissionCollection accessPermissions)
        {
            if (!File.Exists(StorageFileName))
                return;

            // NOTE: this is temporary quick solution
            // Normally permissions should be saved in protected storage, for instance in secured database or encrypted file
            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(StorageFileName, FileMode.Open))
                {
                    var deserialized = formatter.Deserialize(stream) as IAccessPermission[];
                    deserialized.ForEach(accessPermissions.AddOrUpdatePermission);
                }
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while loading permissions from cache", e);
            }
        }

        public void Save(IAccessPermissionCollection accessPermissions)
        {
            // NOTE: this is temporary quick solution
            // Normally permissions should be saved in protected storage, for instance in secured database or encrypted file
            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(StorageFileName, FileMode.Create))
                {
                    var array = accessPermissions.ToArray();
                    formatter.Serialize(stream, array);
                }
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while saving permissions to cache", e);
            }
        }

        public async Task Update(IAccessPermissionCollection accessPermissions)
        {
            // NOTE: this is temporary quick solution
            // Normally it should be replaced with an implementation of partial updates based on MS Sync Framework
            var biometricInfo = await _listUsersBiometricRequest.Request(ListCommand.WithoutParameters);
            var biometricInfoMap = biometricInfo.Users.ToDictionary(x => x.UserName);
            Func<string, UserHash> getHash = x =>
                                             {
                                                 var hash = biometricInfoMap.ContainsKey(x) ? biometricInfoMap[x].BiometricHash : null;
                                                 return new UserHash(x, hash);
                                             };

            // add user permissions
            var accessRights = await _listAccessRightsRequest.Request(ListCommand.WithoutParameters);
            foreach (var userAccessRights in accessRights.UserAccessRights)
            {
                var userName = userAccessRights.UserName;
                var userHash = getHash(userName);

                userAccessRights.PermanentAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new PermanentUserAccess(rule.AccessPointId, userHash);
                        accessPermissions.AddOrUpdatePermission(permission);
                    });

                userAccessRights.ScheduledAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new ScheduledUserAccess(rule.AccessPointId, userHash, rule.WeeklySchedule);
                        accessPermissions.AddOrUpdatePermission(permission);
                    });
            }

            // add user group permissions
            foreach (var groupRights in accessRights.UserGroupAccessRights)
            {
                var groupName = groupRights.UserGroupName;
                var usersInGroupResult = await _listUsersInGroupRequest.Request(ListCommand.ListUsersInGroup(groupName));
                var userHashes = usersInGroupResult.Users.Select(x => getHash(x.UserName)).ToArray();

                groupRights.PermanentAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new PermanentGroupAccess(rule.AccessPointId, groupName, userHashes);
                        accessPermissions.AddOrUpdatePermission(permission);
                    });

                groupRights.ScheduledAccessRules.ForEach(
                    rule =>
                    {
                        var permission = new ScheduledGroupAccess(rule.AccessPointId, groupName, userHashes, rule.WeeklySchedule);
                        accessPermissions.AddOrUpdatePermission(permission);
                    });
            }
        }
    }
}