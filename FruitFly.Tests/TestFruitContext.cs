using fruitfly.core;
using fruitfly.objects;
using NUnit.Framework;

namespace FruitFly.Tests
{
    public class TestFruitContext
    {
        internal static void Init()
        {
            Context.ConsoleWrite = (msg) => TestContext.Progress.WriteLine(msg);
            Context.Config = new Configuration()
            {
                template = "default"
            };
        }
    }
}