using CookBot.BLL.Services.TelegramBot;
using Core.BLL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.BLL.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<PollService>()
                .AddSingleton<ITelegramBotService, TelegramBotService>()
                ;
        }
    }
}