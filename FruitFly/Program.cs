// Pavel Prchal, 2019, 2023

using fruitfly;

if(args.Length != 0)
{
    System.Console.WriteLine("did you expect death-star?");
    return 0;
}
        
Runtime.Start();
var blog = await BlogGenerator.GenerateBlogAsync();
return 0;