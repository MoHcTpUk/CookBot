using CookBot.App.Options;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using CookBot.BLL;
using CookBot.BLL.Services;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendPoolRequest : IRequest<Message>;

    public class BotSendPoolRequestHandler : IRequestHandler<BotSendPoolRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly PollService _pollService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotSendPoolRequestHandler(ITelegramBotService telegramBotService, PollService pollService, IConfiguration configuration)
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

            var message = await _telegramBotService.SendPool(question, options, false, _botOptions.ChatId);

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