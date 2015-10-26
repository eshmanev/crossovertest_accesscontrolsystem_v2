﻿using System.DirectoryServices;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Configuration;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    public class AuthenticationConsumer : IConsumer<IAuthenticateUser>
    {
        private readonly ILdapConfig _config;

        public AuthenticationConsumer(ILdapConfig config)
        {
            _config = config;
        }

        public Task Consume(ConsumeContext<IAuthenticateUser> context)
        {
            try
            {
                var entry = new DirectoryEntry(_config.LdapPath, context.Message.UserName, context.Message.Password);
                // ReSharper disable once UnusedVariable
                var nativeObject = entry.NativeObject;
                return context.RespondAsync(new AuthenticateUserResult(true));
            }
            catch (DirectoryServicesCOMException e)
            {
                return context.RespondAsync(new AuthenticateUserResult(false, e.Message));
            }
            
        }
    }
}