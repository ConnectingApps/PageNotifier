using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PageNotifier.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IManager _manager;
        private readonly ILogger<Worker> _logger;

        public Worker(IManager manager, ILogger<Worker> logger)
        {
            _manager = manager;
            _logger = logger;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            _manager.Initialize("storage.json");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var numberOfUpdates = await _manager.NotifyUpdates();
                _logger.LogInformation($"Number of updates: {numberOfUpdates}");
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Cancellation requested: {time}", DateTimeOffset.Now);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }
    }
}
