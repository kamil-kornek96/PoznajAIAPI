using Serilog.Sinks.MSSqlServer;
using Serilog;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;
using PoznajAI.Services;

namespace PoznajAI.Extensions
{
    public static class HangfireExtension
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));

            services.AddHangfireServer();
            services.AddTransient<HangfireAuthorizationFilter>();



            return services;
        }
    }
}
