using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CookBot.BLL.Services.TelegramBot
{
    public interface ITelegramBotService
    {
        void Start(string botToken, long chatId);
        Task<Message> SendPool(long chatId, string question, string[] options, bool isAnonymous);
        Task<Message> SendMessage(long chatId, string message);
    }
}
