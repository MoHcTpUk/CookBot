using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace CookBot.Class
{
    public static class BotClient
    {
        private static TelegramBotClient Bot { get; set; }
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

            Bot.OnUpdate += BotOnUpdateHandler;
            Bot.StartReceiving();
        }

        public static async Task SendPool()
        {
            await Bot.SendPollAsync(
                chatId: ChatId,
                question: "Будешь завтра кушац?",
                options: new[]
                {
                    "✅ ДА",
                    "⛔️ НЕТ, я сыт багами в коде 🐞"
                },
                isAnonymous: false
            );
        }

        public static async Task SendMenu()
        {
            var now = DateTime.Now;
            var nextDay = now.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                nextDay = now.AddDays(3);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                nextDay = now.AddDays(2);

            var nextDayWeekNumber = new GregorianCalendar().GetWeekOfYear(nextDay, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            var text = @$"Меню на {nextDay.Date:dd-MM-yyyy} ({TranslateDayOfWeek(nextDay.DayOfWeek)}, {(nextDayWeekNumber / 2 == 0 ? "четная" : "не чётная")} неделя):" + Environment.NewLine + Environment.NewLine;

            var menu = MenuRepository.GetMenu(nextDay);

            foreach (var item in menu[nextDay.DayOfWeek])
            {
                text += "🍩 " + item + Environment.NewLine;
            }

            await Bot.SendTextMessageAsync(
                chatId: ChatId,
                text: text,
                parseMode: ParseMode.Markdown
            );
        }

        private static string TranslateDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "понедельник";
                case DayOfWeek.Tuesday:
                    return "вторник";
                case DayOfWeek.Wednesday:
                    return "среда";
                case DayOfWeek.Thursday:
                    return "четверг";
                case DayOfWeek.Friday:
                    return "пятница";
                case DayOfWeek.Saturday:
                    return "суббота";
                case DayOfWeek.Sunday:
                    return "воскресенье";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
            }
        }

        private static void BotOnUpdateHandler(object sender, UpdateEventArgs e)
        {
            Console.WriteLine(e.Update.Id);
        }
    }
}
