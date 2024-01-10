using PoznajAI.Data.Repositories;
using PoznajAI.Services;
using PoznajAI.Services.Video;

namespace PoznajAI.Extensions
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection AddRegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICourseService, CourseService>();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<IVideoConversionService, VideoConversionService>();
            services.AddSingleton<WebSocketService>();

            return services;
        }
    }
}
