using System;
using System.Linq;
using CookBot.BLL.Services.TelegramBot;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CookBot.BLL.DTO;
using CookBot.DAL.Entities;
using Core.BLL.Services;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public class BotClosePoolRequest : IRequest<Poll>
    {
        public int MessageId { get; set; }
    }

    public class BotClosePoolRequestHandler : IRequestHandler<BotClosePoolRequest, Poll>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IService<PollEntity, PollEntityDto> _pollService;

        public BotClosePoolRequestHandler(ITelegramBotService telegramBotService, IService<PollEntity, PollEntityDto> pollService)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
        }

        public async Task<Poll> Handle(BotClosePoolRequest request, CancellationToken cancellationToken)
        {
            var poll = (await _pollService.SelectAsync(pollEntity => pollEntity.MessageId == request.MessageId)).FirstOrDefault();

            if (poll == null)
                throw new Exception("Poll not found!");

            poll.isClosed = true;
            poll.Updated = DateTime.Now;
            await _pollService.UpdateAsync(poll);

            return await _telegramBotService.ClosePool(request.MessageId);
        }
    }
}
