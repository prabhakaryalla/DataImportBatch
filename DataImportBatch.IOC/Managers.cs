using System;
using DataImportBatch.Contracts.Managers;
using DataImportBatch.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportBatch.IOC;

public static class Managers
{
    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddSingleton<IProcessManager, ProcessManager>();
        return services;
    }
}
