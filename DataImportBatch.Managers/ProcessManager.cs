using AutoMapper;
using DataImportBatch.Contracts;
using DataImportBatch.Contracts.Managers;
using DataImportBatch.Contracts.Models;
using DataImportBatch.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataImportBatch.Managers;

public class ProcessManager(ILogger<ProcessManager> logger, IOptions<ConfigurationSettings> options, 
ITransactionService transactionService,IFileService fileService) : IProcessManager
{
   
    public void RunAsync(string fileName)
    {
        var configurationSettings = options.Value;
        
        string sourcePath = $"{configurationSettings.BaseFolderPath}{fileName}";
        string processPath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.ProcessFolder}{fileName}";
        string archivePath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.ArchiveFolder}{fileName}";
        string failedpath = $"{configurationSettings.BaseFolderPath}{configurationSettings.InFromFolder}{configurationSettings.FailedFolder}{fileName}";
       
        File.Move(sourcePath, processPath);

        var transactions =  fileService.GetRecords<TransactionHistoryRawModel>(processPath);
        logger.LogInformation($"Total Records Retrived from file: {transactions.ToList().Count}");
        
        var result = transactionService.ImportTransactionHistoryData(transactions);
        bool isSuccess = !result.Where(s => s.IsProductSaved == false || s.IsTransactionHistorySaved == false).Any();

        
        string outputFilePath = string.Empty;
        if (isSuccess)
        {
            File.Move(processPath, archivePath);
            outputFilePath = $"{configurationSettings.BaseFolderPath}{configurationSettings.OutToFolder}{configurationSettings.ArchiveFolder}{fileName}";
        }
        else
        {
            File.Move(processPath, failedpath);
            outputFilePath = $"{configurationSettings.BaseFolderPath}{configurationSettings.OutToFolder}{configurationSettings.FailedFolder}{fileName}";
        }
        fileService.WriteRecords<TransactionHistoryOutputModel>(result, outputFilePath);
    }
}
