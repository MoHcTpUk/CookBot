using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Telegram.Bot;

namespace CookBot.Class
{
    public static class BotClient
    {
        public static TelegramBotClient Bot { get; private set; }
        private static string BotToken { get; set; }
        public static long ChatId { get; private set; }

        public static void Start()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
                    Program.ConfigFile))
                .Build();

            try
            {
                var botConfiguration = configuration.GetSection("Bot");
                BotToken = botConfiguration.GetValue<string>("BotToken");
                ChatId = botConfiguration.GetValue<long>("ChatId");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Bot' section in " + Program.ConfigFile + ": " + exception.Message);
            }

            Bot = new TelegramBotClient(BotToken);

            Bot.StartReceiving();
        }
    }
}
