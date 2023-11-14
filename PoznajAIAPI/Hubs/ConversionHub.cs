using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace PoznajAI.Hubs
{
    public sealed class ConversionHub : Hub, IConversionHub
    {

        public ConversionHub()
        {

        }
        public async Task SendConversionStatus(string fileName, string status)
        {
            await Clients.All.SendAsync(fileName, status);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }
    }
}
