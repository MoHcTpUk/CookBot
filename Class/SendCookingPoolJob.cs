using Quartz;
using System;
using System.Threading.Tasks;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Send pool");

            await BotClient.SendMenu();
            await BotClient.SendPool();
        }
    }
}