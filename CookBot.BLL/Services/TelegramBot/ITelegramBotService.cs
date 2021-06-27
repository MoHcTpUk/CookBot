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
        Task<Message> SendPool(string question, string[] options, bool isAnonymous);
        Task<Poll> ClosePool(int messageId);
        Task<Message> SendMessage(string message);
        public event EventHandler<UpdateEventArgs> OnUpdate;
    }
}
