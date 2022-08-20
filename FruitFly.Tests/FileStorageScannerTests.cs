using NUnit.Framework;
using fruitfly.core;
using fruitfly.objects;

namespace FruitFly.Tests
{
    [TestFixture]
    public class FileStorageScannerTests
    {
        [Test]
        public void SmokeTest(string content)
        {
            var config = new Configuration();
            var storage = new FileStorage(
                configSource: config,
                configuration: config,
                console: new fruitfly.Console()
            ) as IStorage;
            var blog = storage.Scan();
        }
    }
}