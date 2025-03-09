using DataImportBatch.Contracts.Managers;
using DataImportBatch.Host;
using DataImportBatch.IOC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddManagers();
builder.Services.AddServices();
builder.Services.AddMappers();
builder.Services.AddHostedService(p => new HostedService(args, p.GetRequiredService<ILogger<HostedService>>(), p.GetRequiredService<IProcessManager>()));
var host = builder.Build();
host.Run();
