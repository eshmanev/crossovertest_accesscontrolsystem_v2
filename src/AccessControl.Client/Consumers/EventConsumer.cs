using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Consumers
{
    internal class EventConsumer : IConsumer<IUserBiometricUpdated>,
                                   IConsumer<IPermanentUserAccessAllowed>,
                                   IConsumer<IPermanentGroupAccessAllowed>,
                                   IConsumer<IPermanentUserAccessDenied>,
                                   IConsumer<IPermanentGroupAccessDenied>,
                                   IConsumer<IScheduledUserAccessAllowed>,
                                   IConsumer<IScheduledGroupAccessAllowed>,
                                   IConsumer<IScheduledUserAccessDenied>,
                                   IConsumer<IScheduledGroupAccessDenied>
    {
        private readonly IAccessPermissionCollection _accessPermissions;
        private readonly IAccessPermissionService _service;

        public EventConsumer(IAccessPermissionCollection accessPermissions, IAccessPermissionService service)
        {
            Contract.Requires(accessPermissions != null);
            Contract.Requires(service != null);

            _accessPermissions = accessPermissions;
            _service = service;
        }

        public Task Consume(ConsumeContext<IUserBiometricUpdated> context)
        {
            var visitor = new UpdateUserHashVisitor(_accessPermissions, context.Message.UserName, context.Message.OldBiometricHash, context.Message.NewBiometricHash);
            _accessPermissions.ForEach(x => x.Accept(visitor));
            _service.Save(_accessPermissions);
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserAccessAllowed> context)
        {
            var hash = new UserHash(context.Message.UserName, context.Message.BiometricHash);
            _accessPermissions.AddOrUpdatePermission(new PermanentUserAccess(context.Message.AccessPointId, hash));
            _service.Save(_accessPermissions);
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentGroupAccessAllowed> context)
        {
            var hashes = CombineHashes(context.Message.UsersInGroup, context.Message.UsersBiometrics);
            _accessPermissions.AddOrUpdatePermission(new PermanentGroupAccess(context.Message.AccessPointId, context.Message.UserGroupName, hashes));
            _service.Save(_accessPermissions);
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IScheduledUserAccessAllowed> context)
        {
            var hash = new UserHash(context.Message.UserName, context.Message.BiometricHash);
            var permission = new ScheduledUserAccess(context.Message.AccessPointId, hash, WeeklySchedule.Convert(context.Message.WeeklySchedule));
            _accessPermissions.AddOrUpdatePermission(permission);
            _service.Save(_accessPermissions);
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IScheduledGroupAccessAllowed> context)
        {
            var hashes = CombineHashes(context.Message.UsersInGroup, context.Message.UsersBiometrics);
            var permission = new ScheduledGroupAccess(context.Message.AccessPointId, context.Message.UserGroupName, hashes, WeeklySchedule.Convert(context.Message.WeeklySchedule));
            _accessPermissions.AddOrUpdatePermission(permission);
            _service.Save(_accessPermissions);
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserAccessDenied> context)
        {
            RemovePermission<PermanentUserAccess>(
                x => x.AccessPointId == context.Message.AccessPointId && x.UserHash.UserName.Equals(context.Message.UserName, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentGroupAccessDenied> context)
        {
            RemovePermission<PermanentGroupAccess>(
                x => x.AccessPointId == context.Message.AccessPointId && x.UserGroupName.Equals(context.Message.UserGroupName, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IScheduledUserAccessDenied> context)
        {
            RemovePermission<ScheduledUserAccess>(
                x => x.AccessPointId == context.Message.AccessPointId && x.UserHash.UserName.Equals(context.Message.UserName, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IScheduledGroupAccessDenied> context)
        {
            RemovePermission<ScheduledGroupAccess>(
                x => x.AccessPointId == context.Message.AccessPointId && x.UserGroupName.Equals(context.Message.UserGroupName, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(true);
        }

        private UserHash[] CombineHashes(string[] usersInGroup, string[] userHashes)
        {
            var hashes = new UserHash[usersInGroup.Length];
            for (var i = 0; i < usersInGroup.Length; i++)
            {
                hashes[i] = new UserHash(usersInGroup[i], userHashes[i]);
            }
            return hashes;
        }

        private void RemovePermission<T>(Predicate<T> predicate) where T : IAccessPermission
        {
            var permission = _accessPermissions.OfType<T>().FirstOrDefault(x => predicate(x));
            if (permission != null)
            {
                _accessPermissions.RemovePermission(permission);
                _service.Save(_accessPermissions);
            }
        }
    }
}