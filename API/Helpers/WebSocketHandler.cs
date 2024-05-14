using ClassLibraryJobs;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Text.Json;
using API.Services;

namespace API.Helpers
{
    public class WebSocketHandler
    {
        private readonly JobService _jobService;

        public WebSocketHandler(JobService jobService)
        {
            _jobService = jobService;
        }        

        public async Task HandleClient(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var messageBuffer = new List<byte>();
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    messageBuffer.AddRange(buffer.Take(result.Count));

                    if (result.EndOfMessage)
                    {
                        Message message = Message.Deserialize(messageBuffer.ToArray());

                        switch (message.Action)
                        {
                            case "AddJob":
                                var job = new Job(Guid.NewGuid().ToString(), message.Duration, JobStatus.Pending);
                                _jobService.QueueJob(job);
                                await webSocket.SendAsync(Encoding.UTF8.GetBytes($"JobAdded:{job.GetId()}"), result.MessageType, true, CancellationToken.None);
                                break;
                            case "StartJob":
                                await StartJob(webSocket, message.Id);
                                break;
                            case "CancelJob":
                                await CancelJob(webSocket, message.Id);
                                break;
                        }
                        // Reset message buffer for the next message
                        messageBuffer.Clear();
                    }                                       
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }

            
        }

        private async Task StartJob(WebSocket webSocket, string id)
        {
            Job job = _jobService.GetJob(id);
            if (job != null)
            {
                await webSocket.SendAsync(Encoding.UTF8.GetBytes($"JobStarted:{id}"), WebSocketMessageType.Text, true, CancellationToken.None);
                await job.StartAsync();
                await webSocket.SendAsync(Encoding.UTF8.GetBytes($"JobComplete:{id}"), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task CancelJob(WebSocket webSocket, string id)
        {
            Job job = _jobService.GetJob(id);
            if (job != null)
            {
                job.CancelJob();
                await webSocket.SendAsync(Encoding.UTF8.GetBytes($"JobCancelled:{id}"), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
