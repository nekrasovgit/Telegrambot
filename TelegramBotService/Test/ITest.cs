using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.Test
{
    public interface ITest
    {
        Task UserRegistration(ITelegramBotClient telegramBotClient, Update update);
        Task StartTest(ITelegramBotClient telegramBotClient, Update update);
    }
}
