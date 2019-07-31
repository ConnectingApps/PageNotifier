using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PageNotifier.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IWebsiteReader, WebsiteReader>();
                    services.AddSingleton<IWebsiteShower, WebsiteShower>();
                    services.AddSingleton<IStorage, Storage>();
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<IManager, Manager>();
                    services.AddHostedService<Worker>();
                });
        }
    }
}