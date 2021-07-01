namespace CookBot.App.Options
{
    public class BotOptions
    {
        public const string Bot = "Bot";

        public string BotToken { get; set; }
        public long ChatId { get; set; }
        public long OrderChatId { get; set; }
    }
}
