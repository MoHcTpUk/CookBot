﻿using System.Threading;
using System.Threading.Tasks;
using CookBot.BLL.Services.TelegramBot;
using MediatR;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public class BotSendPoolRequest : IRequest<Message>
    {
        public long ChatId { get; set; }
    }

    public class BotSendPoolRequestHandler : IRequestHandler<BotSendPoolRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;

        public BotSendPoolRequestHandler(ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        public async Task<Message> Handle(BotSendPoolRequest request, CancellationToken cancellationToken)
        {
            string question = "Будешь завтра кушац?";
            string[] options = new[]
            {
                "✅ ДА",
                "⛔️ НЕТ, я сыт багами в коде 🐞"
            };

            return await _telegramBotService.SendPool(request.ChatId, question, options, false);
        }
    }
}