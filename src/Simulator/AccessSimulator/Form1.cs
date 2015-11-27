using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Commands;
using AccessSimulator.AccessCheckServiceProxy;
using MassTransit;

namespace AccessSimulator
{
    public partial class Form1 : Form
    {
        private readonly IBus _bus;

        public Form1(IBus bus)
        {
            _bus = bus;
            InitializeComponent();
            FillData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (accessPointCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select an access point");
                return;
            }

            if (userCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select a user");
                return;
            }

            var accessPoint = ((CacheItem) accessPointCombo.SelectedItem);
            var userHash = ((CacheItem)userCombo.SelectedItem).Id;
            var proxy = new AccessCheckServiceClient();
            try
            {
                if (proxy.TryPass(new CheckAccess { AccessPointId = new Guid(accessPoint.Id), UserHash = userHash }))
                {
                    MessageBox.Show("Welcome to " + accessPoint.Name, "Authorized", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You are not authorized!", "Not authorized", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private async void FillData()
        {
            messageLabel.Show();
            messageLabel.Text = "Loading data...";
            Enabled = false;
            CacheItem[] accessPoints = new CacheItem[0];
            CacheItem[] users = new CacheItem[0];
            try
            {
                accessPointCombo.SelectedItem = null;
                userCombo.SelectedItem = null;
                accessPointCombo.Items.Clear();
                userCombo.Items.Clear();

                var listBiometricsTask = _bus.CreateClient<IListUsersBiometric, IListUsersBiometricResult>(WellKnownQueues.AccessControl).Request(ListCommand.WithoutParameters);
                var listAccessPointsTask = _bus.CreateClient<IListAccessPoints, IListAccessPointsResult>(WellKnownQueues.AccessControl).Request(ListCommand.WithoutParameters);
                await Task.WhenAll(listBiometricsTask, listAccessPointsTask);

                // convert
                accessPoints = listAccessPointsTask.Result.AccessPoints
                                                   .Select(
                                                       x => new CacheItem
                                                       {
                                                           Id = x.AccessPointId.ToString(),
                                                           Name = $"{x.Name} - {SiteName(x.Site)}/{x.Department}"
                                                       })
                                                   .ToArray();

                users = listBiometricsTask.Result.Users
                                          .Where(x => !string.IsNullOrWhiteSpace(x.BiometricHash))
                                          .Select(
                                              x => new CacheItem
                                              {
                                                  Id = x.BiometricHash,
                                                  Name = $"{x.DisplayName} - {SiteName(x.Site)}/{x.Department}"
                                              })
                                          .ToArray();

                // save cache
                CacheData.Save(accessPoints, users);
            }
            catch
            {
                // load from cache
                CacheData.Load(out accessPoints, out users);
            }
            finally
            {
                // update UI
                Invoke(new MethodInvoker(
                    () =>
                    {
                        accessPointCombo.Items.AddRange(accessPoints.Cast<object>().ToArray());
                        userCombo.Items.AddRange(users.Cast<object>().ToArray());
                        Enabled = true;
                        messageLabel.Hide();
                    }));
            }
        }

        private static string SiteName(string site)
        {
            var firstIndex = site.IndexOf("=");
            var lastIndex = site.IndexOf(",");
            if (firstIndex >= 0 && lastIndex > firstIndex)
            {
                firstIndex += 1;
            }
            return site.Substring(firstIndex, lastIndex - firstIndex);
        }
    }
}