using Hangfire.Dashboard;
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
        var httpContext = context.GetHttpContext();
        StringValues authorizationHeader;
        var token = httpContext.Request.Query["token"].ToString();
        if (!String.IsNullOrEmpty(token))
        {
            var user = _jwtService.FastValidateToken(token);
            if (_configuration["Hangfire:AccessId"] == user.Id.ToString())
            {
                return true;
            }

        }
        return false;
    }
}
