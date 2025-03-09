using CsvHelper.Configuration.Attributes;

namespace DataImportBatch.Contracts.Models;

public class TransactionHistoryOutputModel : TransactionHistoryRawModel
{
    [Index(100)]
    public bool IsProductSaved { get; set; }
    [Index(101)]
    public bool IsTransactionHistorySaved { get; set; }
    [Index(102)]
    public string? ErrorMessage { get; set; }
    [Ignore]
    public Guid? Identifier { get; set; }
}
