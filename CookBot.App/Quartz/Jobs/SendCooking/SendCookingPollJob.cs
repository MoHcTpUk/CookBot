using System.Threading.Tasks;
using CookBot.App.Commands.Bot;
using MediatR;
using Quartz;

namespace CookBot.App.Quartz.Jobs.SendCooking
{
    public class SendCookingPollJob : IJob
    {
        private readonly IMediator _mediator;

        public SendCookingPollJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _ = _mediator.Send(new BotSendMenuRequest());
            _ = _mediator.Send(new BotSendPoolRequest());

            return Task.CompletedTask;
        }
    }
}