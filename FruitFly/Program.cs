// Pavel Prchal, 2019

using fruitfly.core;
using fruitfly.objects;
using System;
using System.Linq;

namespace fruitfly
{
    class Program
    {
        IConsole Console;
        Configuration Configuration;
        IStorage Storage;
        DateTime StartTime = DateTime.Now;

        Program()
        {
            Console = new Console() as IConsole;
            Configuration = new Configuration();
            Storage = new FileStorage(
                Configuration,
                Configuration,
                Console
            );
        }
        
        int Run()
        {
            try
            {
                var blog = (new BlogGenerator(
                    configuration: Configuration,
                    storage: Storage,
                    console: Console,
                    converter: new MarkdigHtmlConverter()
                )).GenerateBlogAsync().Result;

                var seconds = new TimeSpan(DateTime.Now.Ticks - StartTime.Ticks).TotalSeconds;
                Console.WriteLine($"{blog.Posts.Count()} generated at: ${seconds} second(s)");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return 1;
            }
        }

        static int Main(string[] args)
        {
            if(args.Length != 0)
            {
                System.Console.WriteLine("did you expect death-star?");
                return 0;
            }

            return new Program().Run();
        }
    }
}
