﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Repository.Menu;
using MediatR;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public class BotSendMenuRequest : IRequest<Message>
    {
    }

    public class BotSendMenuRequestHandler : IRequestHandler<BotSendMenuRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMenuRepository _menuRepository;

        public BotSendMenuRequestHandler(ITelegramBotService telegramBotService, IMenuRepository menuRepository)
        {
            _telegramBotService = telegramBotService;
            _menuRepository = menuRepository;
        }

        public async Task<Message> Handle(BotSendMenuRequest request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var nextDay = now.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                nextDay = now.AddDays(3);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                nextDay = now.AddDays(2);

            var nextDayWeekNumber = new GregorianCalendar().GetWeekOfYear(nextDay, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            var text = @$"Меню на {nextDay.Date:dd-MM-yyyy} ({TranslateDayOfWeek(nextDay.DayOfWeek)}, {(nextDayWeekNumber / 2 == 0 ? "четная" : "не чётная")} неделя):" + Environment.NewLine + Environment.NewLine;

            var menu = _menuRepository.GetMenu(nextDay);

            foreach (var item in menu[nextDay.DayOfWeek])
            {
                text += "🍩 " + item + Environment.NewLine;
            }

            return await _telegramBotService.SendMessage(text);
        }

        private string TranslateDayOfWeek(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "понедельник",
                DayOfWeek.Tuesday => "вторник",
                DayOfWeek.Wednesday => "среда",
                DayOfWeek.Thursday => "четверг",
                DayOfWeek.Friday => "пятница",
                DayOfWeek.Saturday => "суббота",
                DayOfWeek.Sunday => "воскресенье",
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
            };
        }
    }
}
