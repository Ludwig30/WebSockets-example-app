using ClassLibraryJobs;
using System.Net.WebSockets;
using System.Text;

namespace BlazorApp.Services
{
    public class WebSocketsService
    {
        private readonly ClientWebSocket _client = new();        

        public async Task Connect(string url)
        {
            await _client.ConnectAsync(new Uri(url), CancellationToken.None);
        }

        public async Task AddJob(int duration)
        {
            Message message = new() { Action = "AddJob", Duration = duration };
            await SendMessage(message);
        }

        public async Task StartJob(string id)
        {
            Message message = new() { Action = "StartJob", Id = id };
            await SendMessage(message);
        }

        public async Task CancelJob(string id)
        {
            Message message = new() { Action = "CancelJob", Id = id };
            await SendMessage(message);
        }

        private async Task SendMessage(Message message)
        {
            byte[] bytes = message.Serialize();
            if (bytes.Length > 4096)
            {
                List<byte[]> segments = bytes.Chunk(4096).ToList();
                for (int i = 0; i < segments.Count; i++)
                {
                    if (i == segments.Count - 1)
                    {
                        await _client.SendAsync(new ArraySegment<byte>(segments[i]), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        await _client.SendAsync(new ArraySegment<byte>(segments[i]), WebSocketMessageType.Text, false, CancellationToken.None);
                    }
                }
            }
            else
            {
                await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task<string> ReceiveMessage()
        {
            byte[] buffer = new byte[1024 * 4];
            var result = await _client.ReceiveAsync(buffer, CancellationToken.None);
            byte[] data = new byte[result.Count];
            Array.Copy(buffer, data, result.Count);
            return Encoding.UTF8.GetString(data);
        }

        public async Task Disconnect()
        {
            if (_client.State == WebSocketState.Open)
            {
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
        }        
    }
}
