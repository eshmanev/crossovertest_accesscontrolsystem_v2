using NHibernate;

namespace AccessControl.Server.Data.Session
{
    /// <summary>
    /// Represents a session locator.
    /// </summary>
    public interface ISessionLocator
    {
        /// <summary>
        /// Gets the current session.
        /// </summary>
        ISession GetSession();
    }
}