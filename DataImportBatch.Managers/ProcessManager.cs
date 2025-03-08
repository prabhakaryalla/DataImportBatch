using DataImportBatch.Contracts;
using DataImportBatch.Contracts.Managers;
using DataImportBatch.Contracts.Models;
using DataImportBatch.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataImportBatch.Managers;

public class ProcessManager(ILogger<ProcessManager> logger, IOptions<ConfigurationSettings> options, 
ITransactionService transactionService, IFileService fileService) : IProcessManager
{
   
    public void RunAsync()
    {
        var configurationSettings = options.Value;
        string fileName = "Transactions_BadData_Top100.csv";
        string sourcePath = $"{configurationSettings.BaseFolderPath}{fileName}";
        string processPath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.ProcessFolder}{fileName}";
        string archivePath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.ArchiveFolder}{fileName}";
        string failedpath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.FailedFolder}{fileName}";
       
        // File.Move(sourcePath, processPath);
        //bool isSuccess = false;
         

        var transactions =  fileService.GetRecords<TransactionHistoryModel>(processPath);
        logger.LogInformation($"Total Records Retrived from file: {transactions.ToList().Count}");
        var result = transactionService.ImportTransactionHistoryData(transactions);
        
        // if (isSuccess)
        //     File.Move(processPath, archivePath);
        // else
        //     File.Move(processPath, failedpath);

    }
}
