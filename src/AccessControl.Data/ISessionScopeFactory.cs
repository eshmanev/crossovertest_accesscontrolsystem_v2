namespace AccessControl.Data
{
    /// <summary>
    ///     Represents a factory of session scope.
    /// </summary>
    public interface ISessionScopeFactory
    {
        /// <summary>
        ///     Creates a new session scope.
        /// </summary>
        /// <returns></returns>
        ISessionScope Create();
    }
}