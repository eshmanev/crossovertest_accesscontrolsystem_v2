namespace AccessControl.Service.Notifications.Messages
{
    internal class RedeliverSmsMessage
    {
        public string PhoneNumber { get; set; }
        public string Text { get; set; } 
    }
}