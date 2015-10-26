using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class ListBiometricInfo : IListBiometricInfo
    {
        public ListBiometricInfo(string site, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Department = department;
            Site = site;
        }

        public string Department { get; }
        public string Site { get; }
    }
}