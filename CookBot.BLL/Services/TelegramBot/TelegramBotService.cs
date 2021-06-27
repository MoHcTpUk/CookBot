using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace CookBot.BLL.Services.TelegramBot
{
    public class TelegramBotService : ITelegramBotService
    {
        private TelegramBotClient Bot { get; set; }
        private string BotToken { get; set; }
        public long ChatId { get; private set; }

        public event EventHandler<UpdateEventArgs> OnUpdate
        {
            add => Bot.OnUpdate += value;
            remove => Bot.OnUpdate -= value;
        }

        public void Init(string botToken, long chatId)
        {
            BotToken = botToken;
            ChatId = chatId;

            Bot = new TelegramBotClient(BotToken);
        }

        public void StartReceiving()
        {
            Bot.StartReceiving();
        }

        public async Task<Message> SendPool(string question, string[] options, bool isAnonymous)
        {
            return await Bot.SendPollAsync(
                chatId: ChatId,
                question: question,
                options: options,
                isAnonymous: isAnonymous
            );
        }

        public async Task<Message> SendMessage(string message)
        {
            return await Bot.SendTextMessageAsync(
                chatId: ChatId,
                text: message
            );
        }

        public async Task<Poll> ClosePool(int messageId)
        {
            return await Bot.StopPollAsync(
                chatId: ChatId,
                messageId: messageId
            );
        }
    }
}