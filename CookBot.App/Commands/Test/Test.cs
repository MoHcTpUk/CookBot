using CookBot.BLL.DTO;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.BLL.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CookBot.App.Commands.Test
{
    public class TestRequest : IRequest
    {
        public int MessageId { get; set; }
    }

    public class TestRequestHandler : IRequestHandler<TestRequest>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IService<PollEntity, PollEntityDto> _pollService;

        public TestRequestHandler(ITelegramBotService telegramBotService, IService<PollEntity, PollEntityDto> pollService)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
        }

        public async Task<Unit> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            var polls = await _pollService.SelectAsync(_ => true);
            foreach (var poll in polls)
            {
                Console.WriteLine(poll.MessageId);
            }

            return Unit.Value;
        }
    }
}
