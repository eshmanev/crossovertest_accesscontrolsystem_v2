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
        public string UserName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string[] Roles
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                return null;
            }
        }

        public string[] OnBehalfOf
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                return null;
            }
        }
    }
}