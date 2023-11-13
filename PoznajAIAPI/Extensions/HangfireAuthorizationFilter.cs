using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PoznajAI.Services;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public HangfireAuthorizationFilter(IJwtService jwtService, IConfiguration config)
    {
        _jwtService = jwtService;
        _configuration = config;
    }

    public bool Authorize(DashboardContext context)
    {
        // Implement your own authentication logic here
        var httpContext = context.GetHttpContext();
        StringValues authorizationHeader;
        if (httpContext.Request.Headers.TryGetValue("Authorization", out authorizationHeader))
        {
            var token = authorizationHeader.ToString().Replace("Bearer ", "");
            var user = _jwtService.FastValidateToken(token);
            if (_configuration["Hangfire:AccessId"] == user.Id.ToString())
            {
                return true;
            }

        }
        return false;
    }
}
