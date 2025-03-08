using DataImportBatch.Host;
using DataImportBatch.IOC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddManagers();
builder.Services.AddServices();
builder.Services.AddMappers();
builder.Services.AddHostedService<HostedService>();
var host = builder.Build();
host.Run();
