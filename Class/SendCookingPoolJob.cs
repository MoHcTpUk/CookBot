using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        private TelegramBotClient Bot { get; set; }

        private string BotToken { get; set; }
        private long ChatId { get; set; }

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
                    Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
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







            var mediaList = new List<InputMedia>();
            
            var fileStreamList = new List<FileStream>
            {
                File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "img\\1.jpg")),
                File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "img\\2.jpg")),
                //File.OpenRead(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty, "img\\3.jpg")),
            };

            foreach (var fileStream in fileStreamList)
            {
                mediaList.Add(new InputMedia(fileStream,fileStream.Name));
            }

            try
            {
                await Bot.SendMediaGroupAsync( ChatId,);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

             await Bot.SendTextMessageAsync(
                chatId: ChatId,
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