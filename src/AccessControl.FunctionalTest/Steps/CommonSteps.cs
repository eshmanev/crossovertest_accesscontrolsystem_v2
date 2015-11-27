using AccessControl.Client;
using AccessControl.Service;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class CommonSteps
    {
        private ServiceRunner<BusServiceControl> _accessPointService;
        private ServiceRunner<BusServiceControl> _ldapService;
        private ServiceRunner<ClientServiceControl> _clientService;

        [AfterFeature]
        public static void Cleanup()
        {
            //_accessPointService.Run();
        }

        [Given(@"I have started the AccessPoint service")]
        public void GivenIHaveStartedTheAccessPointService()
        {
            _accessPointService = AccessControl.Service.AccessPoint.Program.CreateService();
            _accessPointService.Run(cfg => cfg.SetServiceName("Test.Service.AccessPoint"));
        }

        [Given(@"I have started the LDAP service")]
        public void GivenIHaveStartedTheLDAPService()
        {
            _ldapService = AccessControl.Service.LDAP.Program.CreateService();
            _ldapService.Run(cfg => cfg.SetServiceName("Test.Service.LDAP"));
        }

        [Given(@"I have started the Client service")]
        public void GivenIHaveStartedTheClientService()
        {
            _clientService = AccessControl.Client.Program.CreateService();
            _clientService.Run(cfg => cfg.SetServiceName("Test.Service.Client"));
        }

    }
}