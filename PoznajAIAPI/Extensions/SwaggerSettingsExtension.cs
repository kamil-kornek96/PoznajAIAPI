using Microsoft.OpenApi.Models;

namespace PoznajAI.Extensions
{
    public static class SwaggerSettingsExtension
    {
        public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PoznajAI API", Version = "v1" });

                // Define JWT security scheme
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                // Define JWT security requirement (apply to specific endpoints)
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };

                c.AddSecurityRequirement(securityRequirement);
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "PoznajAI.xml");
                c.IncludeXmlComments(filePath);
            });

            return services;
        }
    }
}
