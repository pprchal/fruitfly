using NUnit.Framework;
using fruitfly;

namespace FruitFly.Tests
{
    [TestFixture]
    public class FileStorageScannerTests
    {
        [Test]
        public void SmokeTest(string content)
        {
            var storage = new fruitfly.plugins.FileStorage() as IStorage;
            var blog = storage.LoadBlog();
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            fruitfly.Runtime.Add<IConfiguration>(new YamlConfiguration());
        }
    }
}