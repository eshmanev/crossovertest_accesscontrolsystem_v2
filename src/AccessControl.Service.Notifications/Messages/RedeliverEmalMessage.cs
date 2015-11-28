namespace AccessControl.Service.Notifications.Messages
{
    internal class RedeliverEmailMessage
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}