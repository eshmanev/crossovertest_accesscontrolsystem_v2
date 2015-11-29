using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class BiometricSteps
    {
        [Given(@"My employee has the following biometric hash ""(.*)""")]
        public void GivenMyEmployeeHasTheFollowingBiometricHash(string hash)
        {
            var userName = AppSettings.ManagedUserName;
            Bus.Request<IUpdateUserBiometric, IVoidResult>(WellKnownQueues.AccessControl, new UpdateUserBiometric(userName, hash));
        }
    }
}