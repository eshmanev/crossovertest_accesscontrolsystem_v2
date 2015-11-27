﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
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
        }

        

        private async void FillData()
        {
            accessPointCombo.Items.Clear();
            userCombo.Items.Clear();

            var listBiometricsTask = _bus.CreateClient<IListUsersBiometric, IListUsersBiometricResult>(WellKnownQueues.AccessControl).Request(ListCommand.Default);
            var listAccessPointsTask = _bus.CreateClient<IListAccessPoints, IListAccessPointsResult>(WellKnownQueues.AccessControl).Request(ListCommand.Default);
            await Task.WhenAll(listBiometricsTask, listAccessPointsTask);

            foreach (var item in listAccessPointsTask.Result.AccessPoints)
            {
                Invoke(new MethodInvoker(() => accessPointCombo.Items.Add(new AccessPointItem(item))));
            }

            foreach (var item in listBiometricsTask.Result.Users.Where(x => !string.IsNullOrWhiteSpace(x.BiometricHash)))
            {
                Invoke(new MethodInvoker(() => userCombo.Items.Add(new UserItem(item))));
            }
        }

        private class AccessPointItem
        {
            public AccessPointItem(IAccessPoint point)
            {
                Id = point.AccessPointId;
                Name = $"{point.Name} - {SiteName(point.Site)}/{point.Department}";
            }

            public Guid Id { get; }
            public string Name { get; }
        }

        private class UserItem
        {
            public UserItem(IUserBiometric user)
            {
                Id = user.BiometricHash;
                Name = $"{user.DisplayName} - {SiteName(user.Site)}/{user.Department}";
            }

            public string Id { get; }
            public string Name { get; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private static string SiteName(string site)
        {
            var firstIndex = site.IndexOf("=");
            var lastIndex = site.IndexOf(",");
            if (firstIndex >= 0 && lastIndex > firstIndex)
                firstIndex += 1;
            return site.Substring(firstIndex, lastIndex - firstIndex);
        }
    }
}