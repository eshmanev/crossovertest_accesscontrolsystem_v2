using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Contracts.Events;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Consumers
{
    internal class EventConsumer : IConsumer<IUserBiometricUpdated>, IConsumer<IPermanentUserAccessAllowed>, IConsumer<IPermanentUserGroupAccessAllowed>
    {
        private readonly IAccessPermissionCollection _accessPermissions;

        public EventConsumer(IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(accessPermissions != null);
            _accessPermissions = accessPermissions;
        }

        public Task Consume(ConsumeContext<IUserBiometricUpdated> context)
        {
            var visitor = new UpdateUserHashVisitor(_accessPermissions, context.Message.UserName, context.Message.OldBiometricHash, context.Message.NewBiometricHash);
            _accessPermissions.ForEach(x => x.Accept(visitor));
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserAccessAllowed> context)
        {
            var hash = new UserHash(context.Message.UserName, context.Message.BiometricHash);
            _accessPermissions.AddOrUpdatePermission(new PermanentUserAccess(context.Message.AccessPointId, hash));
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IPermanentUserGroupAccessAllowed> context)
        {
            var hashes = new UserHash[context.Message.UsersInGroup.Length];
            for (var i = 0; i < context.Message.UsersInGroup.Length; i++)
                hashes[i] = new UserHash(context.Message.UsersInGroup[i], context.Message.UsersBiometrics[i]);

            _accessPermissions.AddOrUpdatePermission(new PermanentGroupAccess(context.Message.AccessPointId, context.Message.UserGroupName, hashes));
            return Task.FromResult(true);
        }
    }
}