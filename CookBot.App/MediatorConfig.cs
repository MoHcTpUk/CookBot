using System;
using CookBot.DAL;
using Core.BLL.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CookBot.App
{
    public static class MediatorConfig
    {
        public static IMediator Mediator { get; }
        public static IServiceProvider ServiceProvider { get; }
        static MediatorConfig()
        {
            Initializator.Init();
            BLL.Initializator.Init();
            Mediator = Configurator.ServiceProvider.GetService<IMediator>();
            ServiceProvider = Configurator.ServiceProvider;
        }
    }
}