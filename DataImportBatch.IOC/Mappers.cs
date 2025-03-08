using System;
using DataImportBatch.Services.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportBatch.IOC;

public static class Mappers
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TransactionMapper));
        return services;
    }

}
