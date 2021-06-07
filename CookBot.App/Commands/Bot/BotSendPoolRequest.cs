﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CookBot.BLL.DTO;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.BLL.Services;
using MediatR;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public class BotSendPoolRequest : IRequest<Message>
    {
    }

    public class BotSendPoolRequestHandler : IRequestHandler<BotSendPoolRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IService<PollEntity, PollEntityDto> _pollService;

        public BotSendPoolRequestHandler(ITelegramBotService telegramBotService, IService<PollEntity, PollEntityDto> pollService)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
        }

        public async Task<Message> Handle(BotSendPoolRequest request, CancellationToken cancellationToken)
        {
            string question = "Будешь кушац?";
            string[] options = new[]
            {
                "✅ ДА",
                "⛔️ НЕТ, я сыт багами в коде 🐞"
            };

            var message = await _telegramBotService.SendPool(question, options, false);

            var newPool = new PollEntityDto()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                MessageId = message.MessageId
            };

            await _pollService.Create(newPool);

            return message;
        }
    }
}