﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;

        public HandleUpdateService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/inline" => SendInlineKeyboard(_botClient, message),
                "/keyboard" => SendReplyKeyboard(_botClient, message),
                "/remove" => RemoveKeyboard(_botClient, message),
                "/request" => RequestContactAndLocation(_botClient, message),
                _ => Usage(_botClient, message)
            };
            Message sentMessage = await action;


        }

        private async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);


            await Task.Delay(500);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
            });

            return await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Choose",
                                                  replyMarkup: inlineKeyboard);
        }




        static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new[]
                {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                })
            {
                ResizeKeyboard = true
            };

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Choose",
                                                  replyMarkup: replyKeyboardMarkup);
        }

        static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Removing keyboard",
                                                  replyMarkup: new ReplyKeyboardRemove());
        }



        static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
        {
            ReplyKeyboardMarkup RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]  {
                new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                },
                });

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Who or Where are you?",
                                                  replyMarkup: RequestReplyKeyboard);
        }

        static async Task<Message> Usage(ITelegramBotClient bot, Message message)
        {
            const string usage = "Usage:\n" +
                                 "/inline   - send inline keyboard\n" +
                                 "/keyboard - send custom keyboard\n" +
                                 "/remove   - remove custom keyboard\n" +
                                 "/photo    - send a photo\n" +
                                 "/request  - request location or contact";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }





        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };


            return Task.CompletedTask;
        }
    }


}


