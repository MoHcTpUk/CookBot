using CookBot.DAL.Entities;
using CookBot.DAL.Repository.Menu;
using Core.DAL.Configuration;
using Core.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.DAL.Repository
{
    public class RepositoriesConfigurator : IRepositoriesConfigurator
    {
        public void ConfigureRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IRepository<PollEntity>, PollRepository>()
                .AddTransient<IMenuRepository, MenuRepository>()
                ;
        }
    }
}