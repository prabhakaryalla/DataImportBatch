using System;

namespace DataImportBatch.Contracts.Models;

public class TransactionHistoryOutputModel : TransactionHistoryModel
{
    public bool IsProductSaved { get; set; }
    public bool IsTransactionHistorySaved { get; set; }
    public string ErrorMessage {get; set;}
}
