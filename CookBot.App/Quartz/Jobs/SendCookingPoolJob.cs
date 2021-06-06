using CookBot.App.Commands.Bot;
using MediatR;
using Quartz;
using System.Threading.Tasks;

namespace CookBot.App.Quartz.Jobs
{
    public class SendCookingPoolJob : IJob
    {
        private readonly IMediator _mediator;

        public SendCookingPoolJob(IMediator mediator)
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