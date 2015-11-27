using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Commands;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            var splash = new Splash();
            splash.Show();
            splash.Progress = "Starting messaging bus";

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
                AuthenticateClient(splash, busControl);
                splash.Dispose();
                Application.Run(new Form1(busControl));
            }
            finally
            {
                busControl.Stop();
            }
        }

        private static void AuthenticateClient(Splash splash, IBusControl busControl)
        {
            try
            {
                splash.Progress = "Authenticating...";
                var authenticateRequest = busControl.CreateClient<IAuthenticateUser, IAuthenticateUserResult>(WellKnownQueues.AccessControl);

                var result = authenticateRequest.Request(
                    new AuthenticateUser(
                        ConfigurationManager.AppSettings["LdapUserName"],
                        ConfigurationManager.AppSettings["LdapPassword"])).Result;

                if (result.Authenticated)
                    busControl.ConnectTicket(result.Ticket);
            }
            catch (AggregateException e)
            {
                MessageBox.Show(e.InnerExceptions.First().Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}