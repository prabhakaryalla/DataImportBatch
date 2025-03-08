using DataImportBatch.Contracts;
using DataImportBatch.Contracts.Managers;
using DataImportBatch.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataImportBatch.Managers;

public class ProcessManager(ILogger<ProcessManager> logger, IOptions<ConfigurationSettings> options, 
ITransactionService transactionService) : IProcessManager
{
    
    public Task RunAsync()
    {
        logger.LogInformation(options.Value.ArchiveFolderName);
        transactionService.SaveTranactionHistoryData();
        return Task.CompletedTask;
    }
}
