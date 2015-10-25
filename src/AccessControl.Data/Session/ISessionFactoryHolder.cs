using NHibernate;

namespace AccessControl.Data.Session
{
    /// <summary>
    /// Provides a session factory.
    /// </summary>
    public interface ISessionFactoryHolder
    {
        ISessionFactory SessionFactory { get; }
    }
}