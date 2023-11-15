using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace PoznajAI.Hubs
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(Message message);
    }
    public  class ConversionHub : Hub<ITypedHubClient>
    {
    }


    public class Message
    {
        public string Type { get; set; }
        public string Information { get; set; }
    }
}
