using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PoznajAI.Configuration;
using PoznajAI.Data.Data;
using PoznajAI.Extensions;
using PoznajAI.Hubs;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowLocalhost4200");

app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
{
    Authorization = new[] { app.Services.CreateScope().ServiceProvider.GetRequiredService<HangfireAuthorizationFilter>() }
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<VideoConversionHub>("/video-conversion-hub");
    endpoints.MapControllers();
});

app.MapControllers();
Log.Information("App started");
app.Run();
