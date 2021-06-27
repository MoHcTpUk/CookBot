using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
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
        private readonly IMongdoDbService<PollEntity> _pollService;

        public TestRequestHandler(ITelegramBotService telegramBotService, IMongdoDbService<PollEntity> pollService)
        {
            _telegramBotService = telegramBotService;
            _pollService = pollService;
        }

        public async Task<Unit> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            await _pollService.CreateAsync(new PollEntity()
            {
                Created = DateTime.Now,
                isClosed = false,
                isDeleted = false,
                MessageId = 666,
                Updated = DateTime.Now
            });

            var data = await _pollService.SelectAsync(_ => true);

            foreach (var item in data)
            {
                Console.WriteLine(item.MessageId);
            }

            //var polls = await _pollService.SelectAsync(_ => true);
            //foreach (var poll in polls)
            //{
            //    Console.WriteLine(poll.MessageId);
            //}

            return Unit.Value;
        }
    }
}
