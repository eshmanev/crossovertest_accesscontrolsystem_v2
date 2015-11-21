﻿using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IGetFullEnumerationChangeBatchContract))]
    public interface IGetFullEnumerationChangeBatch
    {
        uint BatchSize { get; }
        SyncKnowledge KnowledgeForDataRetrieval { get; }
        SyncId LowerEnumerationBound { get; }
    }
}