// Pavel Prchal, 2019, 2023

using System;
using fruitfly;

if(args.Length != 0)
{
    System.Console.WriteLine("did you expect death-star?");
    return 0;
}
        
Runtime.Start();
var blog = await new BlogGenerator().GenerateBlogAsync();
var seconds = new TimeSpan(DateTime.Now.Ticks - Runtime.StartTime.Ticks).TotalSeconds;
Runtime.Console.WriteLine($"{blog.Posts.Count} generated at: {seconds} second(s)");
return 0;