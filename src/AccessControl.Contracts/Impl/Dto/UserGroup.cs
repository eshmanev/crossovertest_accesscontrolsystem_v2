using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class UserGroup : IUserGroup
    {
        public UserGroup(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Name = name;
        }

        public string Name { get; }
    }
}