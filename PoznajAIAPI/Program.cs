using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PoznajAI.Configuration;
using PoznajAI.Data.Data;
using PoznajAI.Data.Repositories;
using PoznajAI.Services;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Text;
using PoznajAI.Extensions;
using Hangfire;
using PoznajAI.Services.Video;
using PoznajAI.Hubs;
using System.Net.WebSockets;
using System.Net;
using PoznajAI.Websockets.Client;

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

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ICourseService, CourseService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddScoped<IVideoConversionService, VideoConversionService>();

builder.Services.AddSerilogLogging(config["ConnectionStrings:DefaultConnection"]);
builder.Services.AddHangfire(config["ConnectionStrings:HangfireConnection"]);

var mapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile<AutoMapperProfiles>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();
builder.Services.AddWebsocketsClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseRouting();
var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
app.UseWebSockets(wsOptions);
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/send")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using(WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                await Send(context, webSocket);
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;//sprawdziæ
        }
    }
    await next();
});

app.UseCors("AllowLocalhost4200");

app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
{
    Authorization = new[] { app.Services.CreateScope().ServiceProvider.GetRequiredService<HangfireAuthorizationFilter>() }
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();





app.MapControllers();
Log.Information("App started");
app.Run();



async Task Send(HttpContext context, WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
    if(result != null)
    {
        while(!result.CloseStatus.HasValue)
        {
            string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer,0,result.Count));
            Console.WriteLine($"clieny says {msg}");
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Server says: {DateTime.UtcNow:f}")),result.MessageType,result.EndOfMessage,System.Threading.CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
            Console.WriteLine(result);
        }
    }
    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, System.Threading.CancellationToken.None);
};
