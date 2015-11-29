using System;
using System.Linq;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Impl.Commands;
using Shouldly;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class LogSteps
    {
        [Then(@"A successful log entry should be created with my employee and access point ""(.*)""")]
        public void ThenASuccessfulLogEntryShouldBeCreatedWithMyEmployeeAndAccessPoint(string name)
        {
            var result = Bus.Request<IListLogs, IListLogsResult>(
                WellKnownQueues.AccessControl,
                ListCommand.ListLogs(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow));
            result.Logs.Count().ShouldBeGreaterThan(0);
            result.Logs.ShouldContain(x => x.AttemptedAccessPoint.Name == name && !x.Failed && x.User.UserName == AppSettings.ManagedUserName);
        }

        [Then(@"A failed log entry should be created with my employee and access point ""(.*)""")]
        public void ThenAFailedLogEntryShouldBeCreatedWithMyEmployeeAndAccessPoint(string name)
        {
            var result = Bus.Request<IListLogs, IListLogsResult>(
                WellKnownQueues.AccessControl,
                ListCommand.ListLogs(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow));
            result.Logs.Count().ShouldBeGreaterThan(0);
            result.Logs.ShouldContain(x => x.AttemptedAccessPoint.Name == name && x.Failed && x.User.UserName == AppSettings.ManagedUserName);
        }

    }
}