using System;
using eShopOnContainers.CatalogService.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using eShopOnContainers.Events;
using MassTransit;
using MassTransit.KafkaIntegration;

namespace eShopOnContainers.CatalogService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<CatalogContext>();
                await new CatalogContextSeed().SeedAsync(dbContext);

                var bus = services.GetRequiredService<IBusControl>();

                await bus.StartAsync();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
