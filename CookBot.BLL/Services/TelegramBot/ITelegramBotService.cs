using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace CookBot.BLL.Services.TelegramBot
{
    public interface ITelegramBotService
    {
        void StartReceiving();
        void Init(string botToken, long chatId);
        Task<Message> SendPool(string question, string[] options, bool isAnonymous, long chatId);
        Task<Poll> ClosePool(int messageId, long chatId);
        Task<Message> SendMessage(string message, long chatId);

        public event EventHandler<UpdateEventArgs> OnUpdate;
    }
}
