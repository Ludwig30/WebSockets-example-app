using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WebsocketsController : ControllerBase
    {
        private readonly WebSocketHandler _webSocketHandler;

        public WebsocketsController(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketHandler.HandleClient(websocket);
            } else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
