using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class ListAccessPointsResult  : IListAccessPointsResult
    {
        public ListAccessPointsResult(IAccessPoint[] accessPoints)
        {
            Contract.Requires(accessPoints != null);
            AccessPoints = accessPoints;
        }

        public IAccessPoint[] AccessPoints { get; }
    }
}