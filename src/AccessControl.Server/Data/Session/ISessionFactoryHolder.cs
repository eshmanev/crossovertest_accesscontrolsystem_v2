using NHibernate;

namespace AccessControl.Server.Data.Session
{
    /// <summary>
    /// Provides a session factory.
    /// </summary>
    public interface ISessionFactoryHolder
    {
        ISessionFactory SessionFactory { get; }
    }
}