using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.FunctionalTest.Services;
using Shouldly;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class NotificationSteps
    {
        [Then(@"I should be notified by email")]
        public void ThenIShouldBeNotifiedByEmail()
        {
            var findResult = Bus.Request<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap, new FindUserByName(AppSettings.TestManagerUserName));
            TestNotificationService.Instance.Emails.ShouldNotBeEmpty();
            TestNotificationService.Instance.Emails.ShouldContain(
                x => x.EmailAddress == findResult.User.Email &&
                     x.Subject == "Access violation occurred");
        }

        [Then(@"I should be notified by sms")]
        public void ThenIShouldBeNotifiedBySms()
        {
            var findResult = Bus.Request<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap, new FindUserByName(AppSettings.TestManagerUserName));
            TestNotificationService.Instance.Sms.ShouldNotBeEmpty();
            TestNotificationService.Instance.Sms.ShouldContain(
               x => x.Number == findResult.User.PhoneNumber &&
                    x.Body.StartsWith("Access violation occurred"));
        }

    }
}