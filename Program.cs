using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;

namespace ArcadeBackend
{
    public class Program
    {
        private static Client hostClient;

        public class UsernameEntry
        {
            public string username { get; set; }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.MapPost("/auth", (UsernameEntry username, AppDbContext dbContext, HttpContext context) =>
            {
                return Results.Accepted();
            });

            app.MapPost("/host", (Client client, HttpContext context) =>
            {
                IPAddress ipAddress = context.Connection.RemoteIpAddress;

                string ip = ipAddress.ToString();

                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }

                client.Host = ip;
                hostClient = client;
            });

            app.MapGet("/connect", () =>
            {
                if (hostClient != null)
                {
                    return Results.Accepted("/connect", hostClient);
                }
                else
                {
                    return Results.Accepted("/connect", null);
                }
            });

            app.Run();
        }
    }
}