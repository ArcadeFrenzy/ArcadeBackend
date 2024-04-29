using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ArcadeBackend
{
    public class Program
    {
        private static List<Client> clients = new List<Client>();

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

            app.MapPost("/auth", (Client client, AppDbContext dbContext) =>
            {
                /*
                User? user = dbContext.Users.Where(user => user.Username.Equals(username)).FirstOrDefault();

                if(user == null)
                {
                    user = new User();
                    user.Username = client.username;

                    dbContext.Users.Add(user);
                    dbContext.SaveChangesAsync();
                }
                */

                var clientsCopy = new List<Client>(clients);
                clients.Add(client);

                return Results.Accepted("/auth", clientsCopy);
            });

            app.Run();
        }
    }
}