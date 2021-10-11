// Pavel Prchal, 2019

using fruitfly.core;
using System;
using System.Linq;

namespace fruitfly
{
    class Program
    {
        static int Main(string[] args)
        {
            var startTime = DateTime.Now;

            var console = new Console() as IConsole;
            if(args.Length != 0)
            {
                console.WriteLine("did you expect death-star?");
                return 0;
            }

            try
            {
                var blog = (new BlogGenerator(
                    storage: new FileStorage(console),
                    console: console,
                    converter: new MarkdigHtmlConverter()
                ) as IBlogGenerator).GenerateBlogAsync().Result;

                var seconds = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks).TotalSeconds;
                console.WriteLine($"{blog.Posts.Count()} generated at: ${seconds} second(s)");
                return 0;
            }
            catch (Exception ex)
            {
                console.WriteLine($"Error: {ex}");
                return 1;
            }
        }
    }
}
