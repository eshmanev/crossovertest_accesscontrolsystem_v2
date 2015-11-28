using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindAccessPointByIdResult : IFindAccessPointByIdResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindAccessPointByIdResult" /> class.
        /// </summary>
        /// <param name="accessPoint">The access point.</param>
        public FindAccessPointByIdResult(IAccessPoint accessPoint)
        {
            AccessPoint = accessPoint;
        }

        /// <summary>
        ///     Gets the access point.
        /// </summary>
        /// <value>
        ///     The access point.
        /// </value>
        public IAccessPoint AccessPoint { get; }
    }
}