using DataImportBatch.Contracts.Services;
using DataImportBatch.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportBatch.IOC;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ITransactionService, TransactionService>();
        return services;
    }

}
