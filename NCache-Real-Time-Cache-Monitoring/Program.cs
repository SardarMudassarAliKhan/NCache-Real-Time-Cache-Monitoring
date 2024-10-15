using Alachisoft.NCache.Client;
using Microsoft.EntityFrameworkCore;
using NCache_Real_Time_Cache_Monitoring.Data;
using NCache_Real_Time_Cache_Monitoring.Repository;

namespace NCache_Real_Time_Cache_Monitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            var configuration = builder.Configuration;

            // Configure Entity Framework with SQLite
            builder.Services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            var cacheName = configuration["NCacheConfig:CacheName"];
            var server = configuration["NCacheConfig:Server"];

            // Configure NCache connection options
            CacheConnectionOptions options = new CacheConnectionOptions
            {
                ServerList = new List<ServerInfo> { new ServerInfo(server, 8250, false) }
            };

            builder.Services.AddSingleton<ICache>(CacheManager.GetCache(cacheName, options));

            builder.Services.AddScoped<ProductRepository>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
