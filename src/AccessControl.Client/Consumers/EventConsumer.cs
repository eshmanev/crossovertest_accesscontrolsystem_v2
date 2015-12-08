using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Contracts.Events;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Consumers
{
    internal class EventConsumer : IConsumer<IUserBiometricUpdated>,
                                   IConsumer<IPermanentUserAccessAllowed>,
                                   IConsumer<IPermanentUserGroupAccessAllowed>,
                                   IConsumer<IPermanentUserAccessDenied>,
                                   IConsumer<IPermanentUserGroupAccessDenied>
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

        public Task Consume(ConsumeContext<IPermanentUserGroupAccessAllowed> context)
        {
            var hashes = new UserHash[context.Message.UsersInGroup.Length];
            for (var i = 0; i < context.Message.UsersInGroup.Length; i++)
                hashes[i] = new UserHash(context.Message.UsersInGroup[i], context.Message.UsersBiometrics[i]);

            _accessPermissions.AddOrUpdatePermission(new PermanentGroupAccess(context.Message.AccessPointId, context.Message.UserGroupName, hashes));
            _service.Save(_accessPermissions);

            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserAccessDenied> context)
        {
            var permission = _accessPermissions
                .OfType<PermanentUserAccess>()
                .FirstOrDefault(x => x.AccessPointId == context.Message.AccessPointId && x.UserHash.UserName == context.Message.UserName);

            if (permission != null)
            {
                _accessPermissions.RemovePermission(permission);
                _service.Save(_accessPermissions);
            }

            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserGroupAccessDenied> context)
        {
            var permission = _accessPermissions
                .OfType<PermanentGroupAccess>()
                .FirstOrDefault(x => x.AccessPointId == context.Message.AccessPointId && x.UserGroupName == context.Message.UserGroupName);

            if (permission != null)
            {
                _accessPermissions.RemovePermission(permission);
                _service.Save(_accessPermissions);
            }

            return Task.FromResult(true);
        }
    }
}