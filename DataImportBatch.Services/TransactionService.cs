using System;
using DataImportBatch.Contracts.Services;
using DataImportBatch.Data.Data;

namespace DataImportBatch.Services;

public class TransactionService(AdWorksContext adWorksContext) : ITransactionService
{
    public Task SaveTranactionHistoryData()
    {
        var data = adWorksContext.Products.ToList();
        return Task.CompletedTask;
    }
}
