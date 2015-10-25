namespace AccessControl.Contracts.Impl
{
    public class GetPasswordHashResult : IGetPasswordHashResult
    {
        public GetPasswordHashResult(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public string PasswordHash { get; }
    }
}