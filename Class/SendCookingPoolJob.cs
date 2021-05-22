using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        private TelegramBotClient Bot { get; set; }

        private string BotToken { get; set; }
        private long ChatId { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Send pool");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty, Program.ConfigFile))
                .Build();

            var botConfiguration = configuration.GetSection("Bot");

            BotToken = botConfiguration.GetValue<string>("BotToken");
            ChatId = botConfiguration.GetValue<long>("ChatId");

            Bot = new TelegramBotClient(BotToken);

            await SendPool();
        }

        private async Task SendPool()
        {
            await Bot.SendPollAsync(
                chatId: ChatId,
                question: "Будешь завтра кушац?",
                options: new[]
                {
                    "✅ ДА, я очень хочу завтра кушоть 🥓🥪🌭",
                    "⛔️ НЕТ, я сыт вашими багами в коде 😥"
                },
                isAnonymous: false
            );
        }
    }
}