using Microsoft.Extensions.DependencyInjection;

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
