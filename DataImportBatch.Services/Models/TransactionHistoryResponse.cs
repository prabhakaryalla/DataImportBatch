using System;
using DataImportBatch.Data.Models;

namespace DataImportBatch.Services.Models;

public class TransactionHistoryResponse : TransactionHistory
{
    public string? ErrorMessage { get; set; }
}
