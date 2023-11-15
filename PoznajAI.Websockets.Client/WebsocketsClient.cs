using Microsoft.Extensions.Configuration;

namespace PoznajAI.Websockets.Client
{
    public interface IWebsocketsClient
    {

    }

    public class WebsocketsClient : IWebsocketsClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WebsocketsClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

    }
}
