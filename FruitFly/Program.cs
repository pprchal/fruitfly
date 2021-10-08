// Pavel Prchal, 2019, 2020

using fruitfly.core;
using fruitfly.objects;
using System.IO;
using YamlDotNet.Serialization;

namespace fruitfly
{
    class Program
    {
        static void Main(string[] args) =>
            new Program().Run(args);

        private void Run(string[] args)
        {
            if(args.Length == 0)
            {
                Context.Config = LoadYamlConfig();
                BlogGenerator.GenerateBlog();
            }
            else
            {
                Context.ConsoleWrite("~o~ did you expect death-star? ~o~");
            }
        }

        private Configuration LoadYamlConfig() =>
            new DeserializerBuilder()
                .Build()
                .Deserialize<Configuration>(File.OpenText(Constants.Config.FileName));
    }
}
