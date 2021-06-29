using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookBot.App.Commands.Bot
{
    public record BotClosePoolRequest(int MessageId) : IRequest<Telegram.Bot.Types.Poll>;

    public class BotClosePoolRequestHandler : IRequestHandler<BotClosePoolRequest, Telegram.Bot.Types.Poll>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMongdoDbService<PollEntity> _pollService;

        public BotClosePoolRequestHandler(ITelegramBotService telegramBotService, IMongdoDbService<PollEntity> pollService)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
        }

        public async Task<Telegram.Bot.Types.Poll> Handle(BotClosePoolRequest request, CancellationToken cancellationToken)
        {
            var poll = (await _pollService.SelectAsync(new() { _ => _.MessageId == request.MessageId })).FirstOrDefault();

            if (poll == null)
                throw new Exception("Poll not found!");

            poll.isClosed = true;
            poll.Updated = DateTime.Now;
            await _pollService.UpdateAsync(poll);

            return await _telegramBotService.ClosePool(request.MessageId);
        }
    }
}
