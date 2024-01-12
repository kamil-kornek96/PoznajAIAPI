using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PoznajAI.Configuration;
using PoznajAI.Configuration.Automapper;
using PoznajAI.Data.Data;
using PoznajAI.Extensions;
using PoznajAI.Websockets.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var mapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile<AutoMapperProfiles>();
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddJwtAuthentication(config);
builder.Services.AddMaxRequestSize();
builder.Services.AddRegisterServices();
builder.Services.AddSerilogLogging(config.GetConnectionString("DefaultConnection"));
builder.Services.AddHangfire(config.GetConnectionString("HangfireConnection"));
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
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSettings();
builder.Services.AddWebsocketsClient();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;  // Dodaj tê liniê, aby Swagger by³ dostêpny bez dodatkowej œcie¿ki
});


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowLocalhost4200");
ConfigManager.ConfigureWebsockets(app);
ConfigManager.ConfigureVideoConversionHub(app);
ConfigManager.ConfigureHangFire(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
Log.Information("App started");
app.Run();