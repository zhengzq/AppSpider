using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Linq;
using Zq.Runner.Handlers;

namespace Zq.Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", false, false)
                        .AddEnvironmentVariables();
                })
                //service
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging((option) =>
                    {
                        option.ClearProviders();
                        option.AddNLog();
                    });

                    services.AddScoped<IHandler, DeDaoHandler>();

                    services.AddScoped<IHandler[]>((provider =>
                    {
                        return provider.GetServices<IHandler>().ToArray();
                    }));

                    //basic usage
                    services.AddHostedService<Processor>();
                });

            builder.RunConsoleAsync().GetAwaiter().GetResult();
        }
    }
}
