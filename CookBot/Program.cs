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
        public static IMediator Cmd = MediatorConfig.Mediator;
        public static IServiceProvider ServiceProvider = MediatorConfig.ServiceProvider;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand());

            SendCookingPoolJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }
    }
}