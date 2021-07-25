using CookBot.App.Commands.Bot;
using MediatR;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using CookBot.BLL;
using CookBot.BLL.Services;

namespace CookBot.App.Quartz.Jobs.CloseAllPoll
{
    public class CloseAllPollJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly PollService _pollService;

        public CloseAllPollJob(IMediator mediator, PollService pollService)
        {
            _mediator = mediator;
            _pollService = pollService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var openedPolls = (await _pollService.SelectAsync(new() { _ => !_.isClosed })).ToList();

            var lastPool = openedPolls.FirstOrDefault(_ => _.MessageId == openedPolls.Max(_ => _.MessageId));

            if (lastPool != null)
            {
                openedPolls.Remove(lastPool);

                var lastPollResult = await _mediator.Send(new BotClosePoolRequest(lastPool.MessageId));

                if (lastPollResult.Options.Length > 0)
                    await _mediator.Send(new BotSendOrderRequest(lastPollResult.Options[0].VoterCount));
            }

            foreach (var openedPoll in openedPolls)
            {
                _ = await _mediator.Send(new BotClosePoolRequest(openedPoll.MessageId));
            }

            Console.WriteLine("Close all poll");
        }
    }
}