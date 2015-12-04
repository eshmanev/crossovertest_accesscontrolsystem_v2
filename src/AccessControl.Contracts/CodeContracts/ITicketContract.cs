using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ITicket" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ITicket))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ITicketContract : ITicket
    {
        public string Domain
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string[] Roles
        {
            get
            {
                Contract.Requires(Contract.Result<string[]>() != null);
                return null;
            }
        }

        public IUser User
        {
            get
            {
                Contract.Requires(Contract.Result<IUser>() != null);
                return null;
            }
        }

        public string[] OnBehalfOf
        {
            get
            {
                Contract.Requires(Contract.Result<string[]>() != null);
                return null;
            }
        }
    }
}