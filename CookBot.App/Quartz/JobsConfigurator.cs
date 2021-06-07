using CookBot.App.Quartz.Jobs;
using CookBot.App.Quartz.Jobs.CloseAllPoll;
using CookBot.App.Quartz.Jobs.SendCooking;
using Core.BLL.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.App.Quartz
{
    public class JobsConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<JobFactory>()
                .AddScoped<SendCookingPollJob>()
                .AddScoped<CloseAllPollJob>();
            ;
        }
    }
}