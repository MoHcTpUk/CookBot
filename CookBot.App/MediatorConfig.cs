using CookBot.DAL;
using Core.BLL.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.App
{
    public static class MediatorConfig
    {
        public static IMediator Mediator { get; }

        static MediatorConfig()
        {
            Initializator.Init();
            Mediator = Configurator.ServiceProvider.GetService<IMediator>();
        }
    }
}