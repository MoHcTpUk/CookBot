using System;
using CookBot.App;
using CookBot.App.Commands.Bot;
using MediatR;
using System.Threading.Tasks;
using CookBot.App.Quartz.Jobs;

namespace CookBot
{
    class Program
    {
        public static IMediator Cmd = CoreConfig.Mediator;
        public static IServiceProvider ServiceProvider = CoreConfig.ServiceProvider;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand());

            SendCookingPoolJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }
    }
}