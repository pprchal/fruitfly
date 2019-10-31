// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            var context = new Context();
            if(args.Length == 0)
            {
                context.GetLogic<BlogGenerator>().GenerateBlog(args);
            }
            else
            {
                context.Console.WriteLine("~o~ did you expect death-star? ~o~");
            }
        }
    }
}
