using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public class BotSendPoolRequest : IRequest<Message>
    {
    }

    public class BotSendPoolRequestHandler : IRequestHandler<BotSendPoolRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMongdoDbService<PollEntity> _pollService;

        public BotSendPoolRequestHandler(ITelegramBotService telegramBotService, IMongdoDbService<PollEntity> pollService)
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

            var newPool = new PollEntity()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                MessageId = message.MessageId
            };

            await _pollService.CreateAsync(newPool);

            return message;
        }
    }
}