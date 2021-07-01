using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CookBot.App.Options;
using CookBot.BLL.Services.TelegramBot;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendOrderRequest(int lanchAmount) : IRequest<Message>;

    public class BotSendOrderRequestHandler: IRequestHandler<BotSendOrderRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IConfiguration _configuration;

        private BotOptions _botOptions;

        public BotSendOrderRequestHandler(ITelegramBotService telegramBotService, IConfiguration configuration)
        {
            _telegramBotService = telegramBotService;
            _configuration = configuration;
        }

        public async Task<Message> Handle(BotSendOrderRequest request, CancellationToken cancellationToken)
        {
            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            var now = DateTime.Now;
            var nextDay = now.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                nextDay = now.AddDays(3);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                nextDay = now.AddDays(2);

            var text = $"Заказ на {nextDay.Date:dd.MM.yyyy} ({CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(nextDay.DayOfWeek).ToLower()})" 
                       + Environment.NewLine + Environment.NewLine +
                      $"Комплесных обедов: {request.lanchAmount}";

            return await _telegramBotService.SendMessage(text, _botOptions.OrderChatId);
        }
    }
}
