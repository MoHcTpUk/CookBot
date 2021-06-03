using CookBot.BLL.DTO;
using CookBot.DAL.Entities;
using Core.BLL.Configuration;
using Core.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.BLL.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IService<PollEntity, PollEntityDto>, PollService>()
                ;
        }
    }
}