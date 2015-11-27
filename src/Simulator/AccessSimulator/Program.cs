using System;
using System.Configuration;
using System.Windows.Forms;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessSimulator
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseBsonSerializer();
                    var host = cfg.Host(
                        new Uri(ConfigurationManager.AppSettings["RabbitMqUrl"]),
                        h =>
                        {
                            h.Username(ConfigurationManager.AppSettings["RabbitMqUserName"]);
                            h.Password(ConfigurationManager.AppSettings["RabbitMqPassword"]);
                        });
                });

            busControl.Start();
            try
            {
                AuthenticateClient(busControl);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(busControl));
            }
            finally
            {
                busControl.Stop();
            }
        }

        private static void AuthenticateClient(IBusControl busControl)
        {
            var authenticateRequest = busControl.CreateClient<IAuthenticateUser, IAuthenticateUserResult>(WellKnownQueues.AccessControl);

            var result = authenticateRequest.Request(
                new AuthenticateUser(
                    ConfigurationManager.AppSettings["LdapUserName"],
                    ConfigurationManager.AppSettings["LdapPassword"])).Result;

            if (result.Authenticated)
                busControl.ConnectSendObserver(new EncryptedTicketPropagator(result.Ticket));
        }
    }
}