using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService.QuestionMarkup
{
    public class QuestionMarkup
    {
        public InlineKeyboardMarkup CreateQuestionMarkup(string secondQuestionDiscription,
                    string threesQuestionDiscription, string foursQuestionDiscription, string nextAnswer)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[] {
                new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "1", callbackData: nextAnswer),
                        InlineKeyboardButton.WithCallbackData(text: "2", callbackData: secondQuestionDiscription),
                        InlineKeyboardButton.WithCallbackData(text: "3", callbackData: threesQuestionDiscription),
                        InlineKeyboardButton.WithCallbackData(text: "4", callbackData: foursQuestionDiscription),
                    },
                });

            return inlineKeyboard;
        }
    }
}
