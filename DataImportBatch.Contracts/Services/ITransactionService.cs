using DataImportBatch.Contracts.Models;

namespace DataImportBatch.Contracts.Services;

public interface ITransactionService
{
     List<TransactionHistoryOutputModel> ImportTransactionHistoryData(List<TransactionHistoryRawModel> transactionHistoryRawModels);
}