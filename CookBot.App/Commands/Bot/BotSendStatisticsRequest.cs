using CookBot.BLL;
using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Bot
{
    public record BotSendStatisticsRequest : IRequest<Message>;

    public class BotSendStatisticsRequestHandler : IRequestHandler<BotSendStatisticsRequest, Message>
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IConfiguration _configuration;
        private readonly PollService _pollService;

        private BotOptions _botOptions;

        public BotSendStatisticsRequestHandler(ITelegramBotService telegramBotService, IConfiguration configuration, PollService pollService)
        {
            _telegramBotService = telegramBotService;
            _configuration = configuration;
            _pollService = pollService;
        }
        public async Task<Message> Handle(BotSendStatisticsRequest request, CancellationToken cancellationToken)
        {
            _botOptions = _configuration.GetSection(BotOptions.Bot).Get<BotOptions>();

            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var allPolls = await _pollService
                .SelectAsync(new List<Expression<Func<PollEntity, bool>>>
                {
                    _ => _.Created >= firstDayOfMonth && _.Created <= lastDayOfMonth
                });

            var dayCount = allPolls.Count;
            var dinnerCount = allPolls.SelectMany(_ => _.VotedYes).Count();
            var noEatCount = allPolls.SelectMany(_ => _.VotedNo).Count();

            var allUserStats = allPolls
                .SelectMany(_ => _.VotedYes)
                .GroupBy(_ => _.Id)
                .Select(_ => new
                {
                    User = _.FirstOrDefault(),
                    Count = _.Select(user => user).Count()
                })
                .OrderByDescending(_ => _.Count)
                .ToList();

            var mostEating = allUserStats.Where(_ => _.Count == allUserStats.FirstOrDefault()?.Count).ToList();
            var lessEating = allUserStats.Where(_ => _.Count == allUserStats.LastOrDefault()?.Count).ToList();

            string stasMessage = $@"Статистика за {firstDayOfMonth.Date.ToShortDateString()} - {lastDayOfMonth.Date.ToShortDateString()}" + Environment.NewLine + Environment.NewLine;

            stasMessage += $@"Всего дней: {dayCount}" + Environment.NewLine;
            stasMessage += $@"Съедено обедов: {dinnerCount} порций" + Environment.NewLine;
            stasMessage += $@"В среднем в день заказано: {dinnerCount / dayCount} порций" + Environment.NewLine;
            stasMessage += $@"Отказались от обедов: {noEatCount} раз" + Environment.NewLine;

            stasMessage += "Больше всех кушали:" + Environment.NewLine;
            foreach (var user in mostEating)
            {
                stasMessage += $@"  {user.User.FirstName}";
                if (!string.IsNullOrEmpty(user.User.UserName))
                    stasMessage += $@"(@{user.User.UserName})";
                stasMessage += $@" кушал {user.Count} раз" + Environment.NewLine;
            }

            stasMessage += "Меньше всех кушали:" + Environment.NewLine;
            foreach (var user in lessEating)
            {
                stasMessage += $@"  {user.User.FirstName}";
                if (!string.IsNullOrEmpty(user.User.UserName))
                    stasMessage += $@"(@{user.User.UserName})";
                stasMessage += $@" кушал {user.Count} раз" + Environment.NewLine;
            }

            var message = await _telegramBotService.SendMessage(stasMessage, _botOptions.ChatId);

            return message;
        }
    }
}
