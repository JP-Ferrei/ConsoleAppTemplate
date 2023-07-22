using ConsoleAppTemplate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File(new JsonFormatter(), "logs/Logs-.txt", rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
    .CreateLogger();

var logger = Log.Logger;

try
{
    logger.Information("Application starting");

    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT")}.json", true, true)
        .AddCommandLine(args)
        .AddEnvironmentVariables();

    DependencyInjection.Inject(builder.Services, builder.Configuration);

    builder.Services
        .AddLogging(configure => configure.AddSerilog(Log.Logger))
        .AddSingleton(Log.Logger)
        .AddHostedService<Worker>()
        .Configure<ConsoleLifetimeOptions>(opt => opt.SuppressStatusMessages = true);

    var app = builder.Build();

    await app.RunAsync();

    app.WaitForShutdown();
}
catch (Exception ex)
{
    logger.Error("exception {@excpetion}", ex);
}
logger.Information("Application is Shutting down");
