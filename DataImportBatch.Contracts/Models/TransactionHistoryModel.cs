using CsvHelper.Configuration.Attributes;

namespace DataImportBatch.Contracts.Models;

public class TransactionHistoryModel
{
    [Index(0)]
    public string? ProductName { get; set; }
    [Index(1)]
    public string? ProductListPrice { get; set; }
    [Index(2)]
    public string? TransactionDate { get; set; }
    [Index(3)]
    public string? TransactionType { get; set; }
    [Index(4)]
    public string? Quantity { get; set; }
    [Index(5)]
    public string? ActualCost { get; set; }
    [Index(6)]
    public string? ModifiedDate { get; set; }
}