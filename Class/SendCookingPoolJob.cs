using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        private Dictionary<DayOfWeek, List<string>> MenuEven { get; set; }
        private Dictionary<DayOfWeek, List<string>> MenuNotEven { get; set; }

        private List<DayOfWeek> ListDayOfWeek { get; } = new()
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Send pool");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
                    Program.ConfigFile))
                .Build();

            try
            {
                MenuEven = GetMenu(configuration.GetSection("Menu").GetSection("Even"));
                MenuNotEven = GetMenu(configuration.GetSection("Menu").GetSection("NotEven"));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while reading 'Menu' section in " + Program.ConfigFile + ": " + exception.Message);
            }

            await SendMenu();
            await SendPool();
        }


        private Dictionary<DayOfWeek, List<string>> GetMenu(IConfigurationSection menuConfigurationSection)
        {
            var menu = new Dictionary<DayOfWeek, List<string>>();

            foreach (var dayOfWeek in ListDayOfWeek)
            {
                var monday = menuConfigurationSection.GetSection(dayOfWeek.ToString());
                var listOfDish = monday.Get<string[]>().ToList();
                menu.Add(dayOfWeek, listOfDish);
            }

            return menu;
        }

        private async Task SendPool()
        {
            await BotClient.Bot.SendPollAsync(
                chatId: BotClient.ChatId,
                question: "Будешь завтра кушац?",
                options: new[]
                {
                    "✅ ДА",
                    "⛔️ НЕТ, я сыт багами в коде 🐞"
                },
                isAnonymous: false
            );
        }

        private async Task SendMenu()
        {
            var now = DateTime.Now;
            var nextDay = now.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                nextDay = now.AddDays(3);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                nextDay = now.AddDays(2);

            var nextDayWeekNumber = new GregorianCalendar().GetWeekOfYear(nextDay, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            var text = @$"Меню на {nextDay.Date:dd-MM-yyyy} ({TranslateDayOfWeek(nextDay.DayOfWeek)}, {(nextDayWeekNumber / 2 == 0 ? "четная" : "не чётная")} неделя):" + Environment.NewLine + Environment.NewLine;

            var menu = nextDayWeekNumber / 2 == 0 ? MenuEven : MenuNotEven;

            foreach (var item in menu[nextDay.DayOfWeek])
            {
                text += "🍩 " + item + Environment.NewLine;
            }

            await BotClient.Bot.SendTextMessageAsync(
                chatId: BotClient.ChatId,
                text: text,
                parseMode: ParseMode.Markdown
            );
        }

        private string TranslateDayOfWeek(DayOfWeek dayOfWeek)
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
    }
}