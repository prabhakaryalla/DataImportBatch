using System;

namespace DataImportBatch.Services.Models;

public class TransactionHistoryData
{
    public int ProductId { get; set; }
    public string? TransactionDate { get; set; }
    public string? TransactionType { get; set; }
    public string? Quantity { get; set; }
    public string? ActualCost { get; set; }
    public string? ModifiedDate { get; set; }

    public string? ErrorMessage {get; set;}
    public Guid? Identifier {get; set;}
}