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
            var storage = new FileStorage(
                new Configuration(),
                new fruitfly.Console()
            ) as IStorage;
            var blog = storage.Scan();
        }
    }
}