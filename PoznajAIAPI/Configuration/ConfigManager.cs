using Hangfire;
using PoznajAI.Hubs;
using PoznajAI.Services;

namespace PoznajAI.Configuration
{
    public static class ConfigManager
    {
        public static void ConfigureWebsockets(WebApplication app)
        {
            var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
            app.UseWebSockets(wsOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/send")
                {
                    WebSocketService webSocketManager = context.RequestServices.GetService<WebSocketService>();
                    await webSocketManager.HandleWebSocketRequest(context);
                }
                else
                {
                    await next();
                }
            });
        }

        public static void ConfigureHangFire(WebApplication app)
        {
            app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
            {
                Authorization = new[] { app.Services.CreateScope().ServiceProvider.GetRequiredService<HangfireAuthorizationFilter>() }
            });
        }

        public static void ConfigureVideoConversionHub(WebApplication app)
        {
            app.MapHub<VideoConversionHub>("/video-conversion-hub");
            app.MapControllers();
        }
    }
}
