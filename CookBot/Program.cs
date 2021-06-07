using CookBot.App;
using CookBot.App.Commands.Bot;
using CookBot.App.Quartz.Jobs.CloseAllPoll;
using CookBot.App.Quartz.Jobs.SendCooking;
using MediatR;
using System;
using System.Threading.Tasks;

namespace CookBot
{
    class Program
    {
        public static IMediator Cmd = CoreConfig.Mediator;
        public static IServiceProvider ServiceProvider = CoreConfig.ServiceProvider;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand());

            SendCookingPollJobScheduler.Start(ServiceProvider);
            CloseAllPollJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }
    }
}