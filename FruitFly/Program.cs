// Pavel Prchal, 2019

using System;
using System.Linq;
using System.Threading.Tasks;

namespace fruitfly
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            if(args.Length != 0)
            {
                System.Console.WriteLine("did you expect death-star?");
                return 0;
            }
            
            Runtime.CreateAndRegister();
            return await RunAsync();
        }

        static async Task<int> RunAsync()
        {
            var console = Runtime.Get<IConsole>();
            
            try
            {
                var blog = await new BlogGenerator().GenerateBlogAsync();
                var seconds = new TimeSpan(DateTime.Now.Ticks - Runtime.StartTime.Ticks).TotalSeconds;
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
