using CookBot.App;
using CookBot.App.Commands.Bot;
using CookBot.App.Commands.Poll;
using CookBot.App.Quartz.Jobs.CloseAllPoll;
using CookBot.App.Quartz.Jobs.SendCooking;
using CookBot.App.Quartz.Jobs.SendStatistics;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CookBot
{
    class Program
    {
        public static IMediator Cmd = CoreConfig.Mediator;
        public static IServiceProvider ServiceProvider = CoreConfig.ServiceProvider;

        public static async Task Main()
        {
            await Cmd.Send(new BotInitializeCommand(new UpdateHandler(Cmd)));

            SendCookingPollJobScheduler.Start(ServiceProvider);
            CloseAllPollJobScheduler.Start(ServiceProvider);
            SendStatisticsJobScheduler.Start(ServiceProvider);

            await Task.Delay(-1);
        }
    }

    public class UpdateHandler : IUpdateHandler
    {
        private readonly IMediator _mediator;

        public UpdateType[] AllowedUpdates { get; }

        public UpdateHandler(IMediator mediator)
        {
            _mediator = mediator;
            AllowedUpdates = new UpdateType[0];
        }

        public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            return update.Type switch
            {
                UpdateType.PollAnswer => _mediator.Send(new PollAddNewVoteCommand(update.PollAnswer), cancellationToken),
                UpdateType.Message => MessageHandler(update),
                _ => Task.CompletedTask
            };
        }

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);

            return Task.CompletedTask;
        }

        private Task MessageHandler(Update update)
        {
            if (update.Message.Entities == null) 
                return Task.CompletedTask;

            var isBotCommand = update.Message.Entities.Select(_ => _.Type).ToList().Contains(MessageEntityType.BotCommand);

            if (isBotCommand)
                _mediator.Send(new BotSendGoEat(update.Message));

            return Task.CompletedTask;
        }
    }
}