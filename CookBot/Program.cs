using CookBot.App;
using CookBot.App.Commands.Bot;
using CookBot.App.Commands.Poll;
using CookBot.App.Quartz.Jobs.CloseAllPoll;
using CookBot.App.Quartz.Jobs.SendCooking;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace CookBot
{
    class Program
    {
        public static IMediator Cmd = CoreConfig.Mediator;
        public static IServiceProvider ServiceProvider = CoreConfig.ServiceProvider;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand(new List<EventHandler<UpdateEventArgs>> { OnUpdateHandler }));

            SendCookingPollJobScheduler.Start(ServiceProvider);
            CloseAllPollJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }

        private static void OnUpdateHandler(object sender, UpdateEventArgs e)
        {
            if (e.Update.Type == UpdateType.PollAnswer)
            {
                Cmd.Send(new PollAddNewVoteCommand(e.Update.PollAnswer));
            }
        }
    }
}