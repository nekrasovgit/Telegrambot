using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotService.TelegramSettings
{
    public class ConfigureWebhook
    {
        private readonly IServiceProvider _services;
        private readonly BotConfiguration _botConfig;

        public ConfigureWebhook(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _services = serviceProvider;
            _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<Telegram.Bot.ITelegramBotClient>();

            var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<Telegram.Bot.ITelegramBotClient>();

            // Remove webhook upon app shutdown
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
