using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PoznajAI.Configuration;
using PoznajAI.Data.Data;
using PoznajAI.Data.Repositories;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audicence"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

    };
});


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILessonCommentService, LessonCommentService>();
builder.Services.AddScoped<ILessonAssignmentService, LessonAssignmentService>();
builder.Services.AddScoped<ILessonRatingService, LessonRatingService>();
builder.Services.AddScoped<ICourseModuleService, CourseModuleService>();
builder.Services.AddScoped<ICourseUserService, CourseUserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILessonCommentRepository, LessonCommentRepository>();
builder.Services.AddScoped<ILessonAssignmentRepository, LessonAssignmentRepository>();
builder.Services.AddScoped<ILessonRatingRepository, LessonRatingRepository>();
builder.Services.AddScoped<ICourseModuleRepository, CourseModuleRepository>();
builder.Services.AddScoped<ICourseUserRepository, CourseUserRepository>();

var mapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile<AutoMapperProfiles>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddSingleton<JwtService>(provider =>
{
    var secretKey = config["JwtSettings:Key"];
    var issuer = config["JwtSettings:Issuer"];
    var audience = config["JwtSettings:Audicence"];
    return new JwtService(secretKey, issuer, audience);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Space API", Version = "v1" });

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
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
