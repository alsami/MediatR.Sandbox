using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MediatR.Sandbox.CustomerServiceApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.RunAsync();
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        // Required for Web-Application Factory!
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static IHost CreateHost(string[] args) =>
            CreateHostBuilder(args)
                .Build();
    }
}