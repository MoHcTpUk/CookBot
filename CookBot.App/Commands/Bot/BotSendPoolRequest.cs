using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendPoolRequest : IRequest<Message>;

    public class BotSendPoolRequestHandler : IRequestHandler<BotSendPoolRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMongdoDbService<PollEntity> _pollService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotSendPoolRequestHandler(ITelegramBotService telegramBotService, IMongdoDbService<PollEntity> pollService, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
            _configuration = configuration;
        }

        public async Task<Message> Handle(BotSendPoolRequest request, CancellationToken cancellationToken)
        {
            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            string question = "Будешь кушац?";
            string[] options = {
                "✅ ДА",
                "⛔️ НЕТ, я сыт багами в коде 🐞"
            };

            var message = await _telegramBotService.SendPool(question, options, false,_botOptions.ChatId);

            var newPool = new PollEntity()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                MessageId = message.MessageId,
                PollId = message.Poll.Id
            };

            await _pollService.CreateAsync(newPool);

            return message;
        }
    }
}