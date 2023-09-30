// Pavel Prchal, 2019, 2023

using System;
using System.Threading.Tasks;
using fruitfly;

if(args.Length != 0)
{
    System.Console.WriteLine("did you expect death-star?");
    return 0;
}
        
Runtime.CreateAndRegister();
return await RunAsync();

static async Task<int> RunAsync()
{
    var console = Runtime.Get<IConsole>();
            
    try
    {
        var blog = await new BlogGenerator().GenerateBlogAsync();
        var seconds = new TimeSpan(DateTime.Now.Ticks - Runtime.StartTime.Ticks).TotalSeconds;
        console.WriteLine($"{blog.Posts.Count} generated at: ${seconds} second(s)");
        return 0;
    }
    catch (Exception ex)
    {
        console.WriteLine($"Error: {ex}");
        return 1;
    }
}

