using CookBot.BLL.Services.TelegramBot;
using CookBot.DAL.Entities;
using Core.BLL.Configuration;
using Core.Module.MongoDb.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.BLL.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IMongdoDbService<PollEntity>, PollService>()
                .AddSingleton<ITelegramBotService, TelegramBotService>()
                ;
        }
    }
}