using Quartz;
using System;
using System.Threading.Tasks;

namespace CookBot.App.Quartz.Jobs
{
    public class SendCookingPoolJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Send pool");

            //_ = mediator.Send(new BotSendMenuRequest());
            //_ = mediator.Send(new BotSendPoolRequest());

            return Task.CompletedTask;
        }
    }
}