using ClassLibraryJobs;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class WebSocketsService
    {
        private readonly ClientWebSocket _client;

        public WebSocketsService()
        {
            _client = new ClientWebSocket();
        }

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
            byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await _client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> ReceiveMessage()
        {
            byte[] buffer = new byte[1024 * 4];
            var result = await _client.ReceiveAsync(buffer, CancellationToken.None);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
