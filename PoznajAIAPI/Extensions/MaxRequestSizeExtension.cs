using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace PoznajAI.Extensions
{
    public static class MaxRequestSizeExtension
    {
        public static IServiceCollection AddMaxRequestSize(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
                options.Limits.MaxRequestBodySize = int.MaxValue;
            })
                .Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                    options.MaxRequestBodySize = int.MaxValue;
                })
                .Configure<FormOptions>(options =>
                {
                    options.ValueLengthLimit = int.MaxValue;
                    options.MultipartBodyLengthLimit = 134217728;
                    options.MemoryBufferThreshold = int.MaxValue;
                });



            return services;
        }
    }
}
