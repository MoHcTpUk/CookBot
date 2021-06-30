using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Repository.Menu;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendMenuRequest : IRequest<Message>;

    public class BotSendMenuRequestHandler : IRequestHandler<BotSendMenuRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMenuRepository _menuRepository;
        private readonly IConfiguration _configuration;

        public BotSendMenuRequestHandler(ITelegramBotService telegramBotService, IMenuRepository menuRepository, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _menuRepository = menuRepository;
            _configuration = configuration;
        }

        public async Task<Message> Handle(BotSendMenuRequest request, CancellationToken cancellationToken)
        {
            var menuOptions = _configuration.GetSection(MenuOptions.Menu).Get<MenuOptions>();

            if (!menuOptions.Enable) 
                return null;

            var now = DateTime.Now;
            var nextDay = now.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                nextDay = now.AddDays(3);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                nextDay = now.AddDays(2);

            var text =
                @$"Меню на {CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(nextDay.DayOfWeek).ToLower()} ({nextDay.Date:dd.MM.yyyy})" +
                Environment.NewLine + Environment.NewLine;

            var menu = _menuRepository.GetMenu(nextDay);

            foreach (var item in menu)
            {
                text += "🥗 " + item + Environment.NewLine;
            }

            return await _telegramBotService.SendMessage(text);
        }
    }
}
