// Pavel Prchal, 2019

using System.Threading.Tasks;
using fruitfly.core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace fruitfly
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if(args.Length != 0)
            {
                System.Console.WriteLine("~o~ did you expect death-star? ~o~");
                return;
            }

            using IHost host = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.Sources.Clear();
                    // cfg.Add(new YamlCounfigSource());
                })
                .ConfigureServices(services =>
                {
                    services
                        .AddSingleton<IConsole, Console>()
                        .AddSingleton<IStorage, FileStorage>()
                        .AddSingleton<IConverter, MarkdigHtmlConverter>()
                        .AddTransient(typeof(IBlogGenerator), typeof(BlogGenerator));
                })
                .Build();

            var bg = host.Services.GetService<IBlogGenerator>();
            bg.GenerateBlog(args);
            await host.RunAsync();
        }
    }
}
