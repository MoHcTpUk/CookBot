using CookBot.BLL.Services.TelegramBot;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendGoEat(Message Message) : IRequest<Message>;

    public class BotSendGoEatHandler : IRequestHandler<BotSendGoEat, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotSendGoEatHandler(ITelegramBotService telegramBotService, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _configuration = configuration;
        }

        public async Task<Message> Handle(BotSendGoEat request, CancellationToken cancellationToken)
        {
            var allowAccess = _telegramBotService.ValidateBotCommandAccess(request.Message);

            if (!allowAccess)
                return null;

            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            if (request.Message.Chat.Id == _botOptions.ChatId)
                await _telegramBotService.DeleteMessage(request.Message);

            var message =
                "Хватит работать - пора кушац =)" + Environment.NewLine +
                "Еда ждёт на кухне" + Environment.NewLine +
                "🌭🥓🥪🍕🥕🍫"
                ;

            return await _telegramBotService.SendMessage(message, _botOptions.ChatId);
        }
    }
}