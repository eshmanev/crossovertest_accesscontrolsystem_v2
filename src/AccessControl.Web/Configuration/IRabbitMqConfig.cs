namespace AccessControl.Web.Configuration
{
    public interface IRabbitMqConfig
    {
        string Url { get; }
        string UserName { get; }
        string Password { get; }
    }
}