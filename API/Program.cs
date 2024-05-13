using API.Controllers;
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
            string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7236");
                                      policy.AllowAnyMethod();
                                      policy.AllowAnyHeader();
                                      policy.AllowCredentials();
                                  });
            });

            builder.Services.AddControllers();            
            builder.Services.AddSingleton<JobService>();

            var app = builder.Build();            

            app.UseCors(MyAllowSpecificOrigins);            
            app.UseWebSockets(webSocketOptions);
            app.Run();
        }
    }
}
