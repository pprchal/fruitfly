using System.IO;
using YamlDotNet.Serialization;

namespace fruitfly
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Out.WriteLine("~o~");
            System.Console.Out.WriteLine("~o~ ~o~  FRUITFLY 1.0 Blog generator");
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            if(args.Length == 0)
            {
                new BlogGenerator(Context).GenerateBlog();
            }
            else
            {
                System.Console.Out.WriteLine("~o~ do you expect death-star? ~o~");
            }
        }

        private Context _Context = null;
        private Context Context
        {
            get
            {
                if(_Context == null)
                {
                    _Context = new Context();
                    _Context.Config = new DeserializerBuilder()
                        .Build()
                        .Deserialize<Configuration>(new StringReader(File.ReadAllText(Global.CONFIG_YML)));
                }

                return _Context;
            }
        }
    }
}
