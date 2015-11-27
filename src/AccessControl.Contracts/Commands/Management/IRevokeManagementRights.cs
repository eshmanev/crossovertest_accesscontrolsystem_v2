﻿using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    [ContractClass(typeof(IRevokeManagementRightsContract))]
    public interface IRevokeManagementRights
    {
        /// <summary>
        ///     Gets the name of the user who are granted managment rights.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}