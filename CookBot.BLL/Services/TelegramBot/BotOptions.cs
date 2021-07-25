using System.Collections.Generic;

namespace CookBot.BLL.Services.TelegramBot
{
    public class BotOptions
    {
        public const string Bot = "Bot";

        public List<long> AdminList { get; set; }
        public string BotToken { get; set; }
        public long ChatId { get; set; }
        public long OrderChatId { get; set; }
    }
}
