namespace AccessControl.Service.Notifications.Messages
{
    internal class RedeliverEmailMessage
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}