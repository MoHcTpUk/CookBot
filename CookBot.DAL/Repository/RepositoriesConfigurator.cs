using CookBot.DAL.Entities;
using CookBot.DAL.Repository.Menu;
using Core.DAL.Configuration;
using Core.Module.MongoDb.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.DAL.Repository
{
    public class RepositoriesConfigurator : IRepositoriesConfigurator
    {
        public void ConfigureRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IMongoDbRepository<PollEntity>, PollRepository>()
                .AddTransient<IMenuRepository, MenuRepository>()
                ;
        }
    }
}