using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using CookBot.BLL.Services;
using Microsoft.Extensions.Configuration;

namespace CookBot.App.Commands.Bot
{
    public record BotClosePoolRequest(int MessageId) : IRequest<Telegram.Bot.Types.Poll>;

    public class BotClosePoolRequestHandler : IRequestHandler<BotClosePoolRequest, Telegram.Bot.Types.Poll>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly PollService _pollService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotClosePoolRequestHandler(ITelegramBotService telegramBotService, PollService pollService, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
            _configuration = configuration;
        }

        public async Task<Telegram.Bot.Types.Poll> Handle(BotClosePoolRequest request, CancellationToken cancellationToken)
        {
            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            var poll = (await _pollService.SelectAsync(new() { _ => _.MessageId == request.MessageId })).FirstOrDefault();

            if (poll == null)
                throw new Exception("Poll not found!");

            poll.isClosed = true;
            poll.Updated = DateTime.Now;
            await _pollService.UpdateAsync(poll);

            return await _telegramBotService.ClosePool(request.MessageId, _botOptions.ChatId);
        }
    }
}
