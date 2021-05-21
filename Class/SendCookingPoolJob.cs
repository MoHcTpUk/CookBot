using System;
using System.Threading.Tasks;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        private TelegramBotClient Bot { get; } = new(Configurations.BotToken);

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Send pool");
            await SendPool();
        }

        private async Task SendPool()
        {
            Bot.StartReceiving(Array.Empty<UpdateType>());

            await Bot.SendPollAsync(
                chatId: -1001180471941,
                question: "Будешь сегодня кушац?",
                options: new[]
                {
                    "Да, я очень хочу сегодня кушоть 🥓🥪🌭",
                    "Нет, я сыт вашими багами в коде 😥"
                },
                isAnonymous: false
            );

            Bot.StopReceiving();
        }
    }
}