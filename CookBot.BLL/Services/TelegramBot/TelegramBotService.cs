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

        public event EventHandler<UpdateEventArgs> OnUpdate
        {
            add => Bot.OnUpdate += value;
            remove => Bot.OnUpdate -= value;
        }

        public void Init(string botToken, long chatId)
        {
            BotToken = botToken;

            Bot = new TelegramBotClient(BotToken);
        }

        public void StartReceiving()
        {
            Bot.StartReceiving();
        }

        public async Task<Message> SendPool(string question, string[] options, bool isAnonymous, long chatId)
        {
            return await Bot.SendPollAsync(
                chatId: chatId,
                question: question,
                options: options,
                isAnonymous: isAnonymous
            );
        }

        public async Task<Message> SendMessage(string message, long chatId)
        {
            return await Bot.SendTextMessageAsync(
                chatId: chatId,
                text: message
            );
        }

        public async Task<Poll> ClosePool(int messageId, long chatId)
        {
            return await Bot.StopPollAsync(
                chatId: chatId,
                messageId: messageId
            );
        }
    }
}