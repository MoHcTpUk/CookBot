using CookBot.App;
using CookBot.App.Commands.Bot;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBot.App.Commands.Poll;
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
            await Cmd.Send(new BotInitializeCommand(new List<EventHandler<UpdateEventArgs>> { OnNewVote }));

            var msg = await Cmd.Send(new BotSendPoolRequest());

            //var poll = await Cmd.Send(new BotClosePoolRequest(msg.MessageId));

            //await Cmd.Send(new TestRequest());

            //SendCookingPollJobScheduler.Start(ServiceProvider);
            //CloseAllPollJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }

        private static void OnNewVote(object sender, UpdateEventArgs e)
        {
            if (e.Update.Type == UpdateType.PollAnswer)
            {
                Console.WriteLine(@$"{e.Update.PollAnswer.User.Id}: {e.Update.PollAnswer.OptionIds.FirstOrDefault()}");
                Cmd.Send(new PollAddNewVoteCommand(e.Update.PollAnswer));
            }
        }
    }
}