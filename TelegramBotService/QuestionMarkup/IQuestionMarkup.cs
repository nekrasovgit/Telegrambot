using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService.QuestionMarkup
{
    public interface IQuestionMarkup
    {
        InlineKeyboardMarkup CreateQuestionMarkup(string secondQuestionDiscription,
                    string threesQuestionDiscription, string foursQuestionDiscription, string nextAnswer);
    }
}
