using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CookBot.BLL.Services.TelegramBot
{
    public class TelegramBotService : ITelegramBotService
    {
        private TelegramBotClient Bot { get; set; }
        private BotOptions BotOptions { get; set; }

        public void Init(BotOptions options)
        {
            BotOptions = options;

            Bot = new TelegramBotClient(BotOptions.BotToken);
        }

        public void StartReceiving(IUpdateHandler updateHandler)
        {
            Bot.StartReceiving(updateHandler);
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

        public async Task DeleteMessage(Message message)
        {
            await Bot.DeleteMessageAsync(message.Chat.Id,message.MessageId);
        }

        public bool ValidateBotCommandAccess(Message message)
        {
            return BotOptions.AdminList.Contains(message.From.Id);
        }
    }
}