﻿using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IGetChangeBatchContract))]
    public interface IGetChangeBatch
    {
        uint BatchSize { get; }
        SyncKnowledge DestinationKnowledge { get; }
    }
}