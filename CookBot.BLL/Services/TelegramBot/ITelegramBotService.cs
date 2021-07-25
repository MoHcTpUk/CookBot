using System.Threading.Tasks;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CookBot.BLL.Services.TelegramBot
{
    public interface ITelegramBotService
    {
        void StartReceiving(IUpdateHandler updateHandler);
        void Init(string botToken, long chatId);
        Task<Message> SendPool(string question, string[] options, bool isAnonymous, long chatId);
        Task<Poll> ClosePool(int messageId, long chatId);
        Task<Message> SendMessage(string message, long chatId);
        Task DeleteMessage(Message message);
    }
}
