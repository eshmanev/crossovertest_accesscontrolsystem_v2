using System.Configuration;

namespace AccessControl.FunctionalTest
{
    public class AppSettings
    {
        public static string Domain => ConfigurationManager.AppSettings["Domain"];
        public static string TestManagerUserName => ConfigurationManager.AppSettings["TestManagerUserName"];
        public static string TestManagerPassword => ConfigurationManager.AppSettings["TestManagerPassword"];
        public static string TestClientServiceUserName => ConfigurationManager.AppSettings["TestClientServiceUserName"];
        public static string TestClientServicePassword => ConfigurationManager.AppSettings["TestClientServicePassword"];
        public static string ManagedUserName => ConfigurationManager.AppSettings["ManagedUserName"];
        public static string ManagedUserPassword => ConfigurationManager.AppSettings["ManagedUserPassword"];
        public static string ManagedUserGroup => ConfigurationManager.AppSettings["ManagedUserGroup"];
    }
}