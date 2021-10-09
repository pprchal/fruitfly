// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly
{
    class Program
    {
        static int Main(string[] args)
        {
            var console = new Console() as IConsole;
            if(args.Length != 0)
            {
                console.WriteLine("~o~ did you expect death-star? ~o~");
                return 0;
            }

            (new BlogGenerator(
                storage: new FileStorage(console),
                console: console,
                converter: new MarkdigHtmlConverter()
            ) as IBlogGenerator).GenerateBlog(args);
            return 0;
        }
    }
}
