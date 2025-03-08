using DataImportBatch.Contracts.Managers;

namespace DataImportBatch.Host;

public class HostedService(ILogger<HostedService> logger, IProcessManager processManager) : IHostedService
{
    private readonly Task _completedTask = Task.CompletedTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Service} is running.", nameof(HostedService));
        processManager.RunAsync();
        return _completedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
         logger.LogInformation("{Service} is stopping.", nameof(HostedService));
         return _completedTask;
    }
}