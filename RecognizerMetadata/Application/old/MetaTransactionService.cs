using System;
using System.Transactions;
using Application.Services.Interfaces;

namespace Application.Services;

public class MetaTransactionService : IMetaTransactionService
{
    protected TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions 
            { 
                IsolationLevel = level, 
                Timeout = TimeSpan.FromSeconds(5) 
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

}
