using DataImportBatch.Contracts.Models;

namespace DataImportBatch.Contracts.Services;

public interface ITransactionService
{
    (bool isSuccess, List<TransactionHistoryModel> successRecords, List<TransactionHistoryModel> failedRecords) ImportTransactionHistoryData(List<TransactionHistoryModel> transactionHistoryModels);
}