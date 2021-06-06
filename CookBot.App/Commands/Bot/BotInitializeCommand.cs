using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using CookBot.BLL.Services.TelegramBot;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CookBot.App.Commands.Bot
{
    public class BotInitializeCommand : IRequest
    {
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

            _telegramBotService.Start(
                botToken:_botOptions.BotToken, 
                chatId:_botOptions.ChatId);

            return Task.FromResult(Unit.Value);
        }
    }
}
