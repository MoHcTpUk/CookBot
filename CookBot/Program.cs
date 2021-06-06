using CookBot.App;
using CookBot.App.Commands.Bot;
using MediatR;
using System.Threading.Tasks;
using CookBot.App.Commands.Scheduler;

namespace CookBot
{
    class Program
    {
        public static IMediator Cmd = MediatorConfig.Mediator;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand());
            await Cmd.Send(new SchedulerAutoPostPollSetupRequest());

            await Task.Delay(-1);
        }
    }
}