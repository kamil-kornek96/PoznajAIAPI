using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;

namespace PoznajAI.Extensions
{
    public static class SerilogExtension
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection services, string connectionString)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" }
                )
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            return services;
        }
    }

}
