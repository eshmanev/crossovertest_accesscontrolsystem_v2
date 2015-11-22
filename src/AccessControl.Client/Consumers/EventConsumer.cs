using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Contracts.Events;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Client.Consumers
{
    internal class EventConsumer : IConsumer<IUserBiometricUpdated>
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
    }
}