using Core.DAL.Configuration;
using Core.Module.MongoDb;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.DAL.MongoDb
{
    public class ContextFactoryConfigurator : IContextFactoryConfigurator
    {
        public void ConfigureContextFactory(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMongoDatabaseFactory, MongoDatabaseFactory>();
        }
    }
}