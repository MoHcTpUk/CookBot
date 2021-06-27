using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using CookBot.BLL.Services.TelegramBot;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Args;

namespace CookBot.App.Commands.Bot
{
    public class BotInitializeCommand : IRequest
    {
        public List<EventHandler<UpdateEventArgs>> OnUpdateHandlers { get; set; }
    }

    public class BotInitializeCommandHandler: IRequestHandler<BotInitializeCommand>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotInitializeCommandHandler(ITelegramBotService telegramBotService, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _configuration = configuration;
        }

        public Task<Unit> Handle(BotInitializeCommand request, CancellationToken cancellationToken)
        {
            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            _telegramBotService.Init(
                botToken:_botOptions.BotToken, 
                chatId:_botOptions.ChatId);

            foreach (var onUpdateHandler in request.OnUpdateHandlers)
            {
                _telegramBotService.OnUpdate += onUpdateHandler;
            }

            _telegramBotService.StartReceiving();

            return Task.FromResult(Unit.Value);
        }
    }
}
