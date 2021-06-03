using CookBot.DAL.Entities;
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
                .AddSingleton<IRepository<PollEntity>, PollRepository>()
                ;
        }
    }
}