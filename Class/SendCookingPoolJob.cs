using Quartz;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CookBot.Class
{
    public class SendCookingPoolJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Send pool");

            await SendMenu();
            await SendPool();
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

            var menu = MenuRepository.GetMenu(nextDay);

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