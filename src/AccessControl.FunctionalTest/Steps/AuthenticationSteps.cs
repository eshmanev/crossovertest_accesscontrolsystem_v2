using System;
using System.Configuration;
using System.Security;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl.Commands;
using MassTransit;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Service.Security;
using TechTalk.SpecFlow;
using AccessControl.Service.Configuration;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class AuthenticationSteps
    {
        [Given("I'm a Manager")]
        public void AuthenticateManager()
        {
            Authenticate("TestManagerUserName", "TestManagerPassword");
        }

        [Given("I'm a Client Service")]
        public void AuthenticateClientService()
        {
            Authenticate("TestClientServiceUserName", "TestClientServicePassword");
        }

        private void Authenticate(string userNameKey, string passwordKey)
        {
            var serviceConfig = (IServiceConfig)ConfigurationManager.GetSection("service");
            var userName = ConfigurationManager.AppSettings[userNameKey];
            var password = ConfigurationManager.AppSettings[passwordKey];

            var url = new Uri(serviceConfig.RabbitMq.GetQueueUrl(WellKnownQueues.AccessControl));
            IRequestClient<IAuthenticateUser, IAuthenticateUserResult> request = new MessageRequestClient<IAuthenticateUser, IAuthenticateUserResult>(Bus.Instance, url, TimeSpan.FromSeconds(30));
            var result = request.Request(new AuthenticateUser(userName, password)).Result;
            if (!result.Authenticated)
                throw new SecurityException($"Invalid credentials specified. Please check these settings {userNameKey} and {passwordKey} in the app.config file.");

            // take care of automatical request authentication
            Bus.Instance.ConnectTicket(result.Ticket);
        }
    }
}