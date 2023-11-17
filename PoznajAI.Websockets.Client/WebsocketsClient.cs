using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;

namespace PoznajAI.Websockets.Client
{
    public interface IWebsocketsClient
    {
        void SendMessage(string message);
    }

    public class WebsocketsClient : IWebsocketsClient
    {


        public WebsocketsClient()
        {

        }


        public async void SendMessage(string message)
        {
            using(ClientWebSocket client = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("wss://localhost:44376/send");
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(120));
                try
                {
                    await client.ConnectAsync(serviceUri, cancellationTokenSource.Token);
                    var n = 0;
                    while(client.State == WebSocketState.Open)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(byteToSend,WebSocketMessageType.Text,true,cancellationTokenSource.Token);
                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;
                            while (true)
                            {
                                ArraySegment<byte> byteRecievied = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await client.ReceiveAsync(byteRecievied, cancellationTokenSource.Token);
                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                Console.WriteLine(responseMessage);
                                if (response.EndOfMessage)
                                    break;
                            }
                        }
                    }
                }
                catch (WebSocketException ex)
                {
                    //dodać logger serilog jako argument
                }
            }
        }
    }
}
