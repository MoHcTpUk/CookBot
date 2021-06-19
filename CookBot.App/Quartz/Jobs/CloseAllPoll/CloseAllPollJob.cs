using MediatR;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using CookBot.App.Commands.Bot;
using CookBot.BLL.DTO;
using CookBot.DAL.Entities;
using Core.BLL.Services;

namespace CookBot.App.Quartz.Jobs.CloseAllPoll
{
    public class CloseAllPollJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IService<PollEntity, PollEntityDto> _pollService;

        public CloseAllPollJob(IMediator mediator, IService<PollEntity,PollEntityDto> pollService)
        {
            _mediator = mediator;
            _pollService = pollService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var openedPolls = (await _pollService.SelectAsync(poll => !poll.isClosed)).ToList();

            foreach (var openedPoll in openedPolls)
            {
                _ = _mediator.Send(new BotClosePoolRequest { MessageId = openedPoll.MessageId });
            }
            Console.WriteLine("Close all poll");
        }
    }
}