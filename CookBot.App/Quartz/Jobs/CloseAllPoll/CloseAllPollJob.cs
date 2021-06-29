using CookBot.App.Commands.Bot;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookBot.App.Quartz.Jobs.CloseAllPoll
{
    public class CloseAllPollJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IMongdoDbService<PollEntity> _pollService;

        public CloseAllPollJob(IMediator mediator, IMongdoDbService<PollEntity> pollService)
        {
            _mediator = mediator;
            _pollService = pollService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var openedPolls = (await _pollService.SelectAsync(new(){_=>!_.isClosed})).ToList();

            foreach (var openedPoll in openedPolls)
            {
                _ = _mediator.Send(new BotClosePoolRequest(openedPoll.MessageId));
            }
            Console.WriteLine("Close all poll");
        }
    }
}