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
            if(args.Length == 0)
            {
                Context.GetLogic<BlogGenerator>().GenerateBlog(args);
            }
            else
            {
                Context.Console.WriteLine("~o~ did you expect death-star? ~o~");
            }
        }

        private Context Context
        {
            get;
        } = Context.CreateContext();
    }
}
