using System;
using Core.BLL.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.App
{
    public static class CoreConfig
    {
        public static IMediator Mediator { get; }
        public static IServiceProvider ServiceProvider { get; }
        static CoreConfig()
        {
            DAL.Initializator.Init();
            BLL.Initializator.Init();
            Mediator = Configurator.ServiceProvider.GetService<IMediator>();
            ServiceProvider = Configurator.ServiceProvider;
        }
    }
}