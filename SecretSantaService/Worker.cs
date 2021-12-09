using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecretSantaBot;
using SecretSantaService.Implementation;

namespace SecretSantaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private SecretSantaBot.SecretSantaBot bot;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            var telegram = new Telegram.Telegram();
            SecretSantaBot.Extension.TelegramDriver = new TelegramDriver(telegram);
            bot = new SecretSantaBot.SecretSantaBot(new TelegramDriver(telegram), new ICommandHandler[] { new SecretCommandHandler(), new PrivateRoomCommandHandler() }, logger);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                bot.Update();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
