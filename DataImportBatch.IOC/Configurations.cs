using System;
using DataImportBatch.Contracts;
using DataImportBatch.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportBatch.IOC;

public static class Configurations
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConfigurationSettings>(configuration.GetSection("BatchConfiguration"));
        services.AddDbContext<AdWorksContext>(options => options.UseSqlServer(configuration.GetConnectionString("AdWorksConnectionString")), ServiceLifetime.Singleton);
        return services;
    }
}
