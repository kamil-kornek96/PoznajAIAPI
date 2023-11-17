using Microsoft.AspNetCore.SignalR;

namespace PoznajAI.Hubs
{

    public class VideoConversionHub : Hub
    {
        public async Task SendConversionProgress(string fileName, int progress)
        {
            await Clients.All.SendAsync("ConversionProgress", new { FileName = fileName, Progress = progress });
        }
    }

}
