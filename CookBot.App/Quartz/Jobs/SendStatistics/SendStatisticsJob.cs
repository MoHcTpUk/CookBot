using System.Threading.Tasks;
using CookBot.App.Commands.Bot;
using MediatR;
using Quartz;

namespace CookBot.App.Quartz.Jobs.SendStatistics
{
    public class SendStatisticsJob : IJob
    {
        private readonly IMediator _mediator;

        public SendStatisticsJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new BotSendStatisticsRequest());
        }
    }
}