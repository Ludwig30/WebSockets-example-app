using API.Controllers;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.WebSockets;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSingleton<JobService>();
            builder.Services.AddSingleton<WebSocketHandler>();

            var app = builder.Build();            

            app.UseWebSockets(webSocketOptions);
            app.MapControllers();
            app.Run();
        }
    }
}
