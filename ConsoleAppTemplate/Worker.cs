using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleAppTemplate
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger logger;

        public Worker(ILogger logger)
        {
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.Information("Start async");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.Information("teste");
        }

        public override Task StopAsync(CancellationToken token)
        {
            logger.Information("stopAsync");
            return base.StopAsync(token);
        }

        public override void Dispose()
        {
            logger.Information("Dispose");
            base.Dispose();
        }
    }
}
