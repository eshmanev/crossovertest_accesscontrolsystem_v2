﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRegisterAccessPoint" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRegisterAccessPoint))]
    // ReSharper disable once InconsistentNaming
    internal abstract class RegisterAccessPointContract : IRegisterAccessPoint
    {
        public IAccessPoint AccessPoint
        {
            get
            {
                Contract.Ensures(Contract.Result<IAccessPoint>() != null);
                return null;
            }
        }
    }
}