using Core.DAL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.DAL.EF
{
    public class ContextFactoryConfigurator : IContextFactoryConfigurator
    {
        public void ConfigureContextFactory(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<ApplicationDbContext, ExsampleContextFactory>();
        }
    }
}