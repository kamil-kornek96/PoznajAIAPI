using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace PoznajAI.Websockets.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebsocketsClient(
            this IServiceCollection services)
        {
            services.AddTransient<IWebsocketsClient, WebsocketsClient>();

            return services;
        }
    }
}
