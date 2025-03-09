using DataImportBatch.Contracts.Managers;

namespace DataImportBatch.Host;

public class HostedService(string[] args, ILogger<HostedService> logger, IProcessManager processManager) : IHostedService
{
    private readonly Task _completedTask = Task.CompletedTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Service} is running.", nameof(HostedService));
        string fileName = args[0];
        Console.WriteLine($"Args----------------------------------------------------------{fileName}");
        processManager.RunAsync(fileName);
        return _completedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
         logger.LogInformation("{Service} is stopping.", nameof(HostedService));
         return _completedTask;
    }
}