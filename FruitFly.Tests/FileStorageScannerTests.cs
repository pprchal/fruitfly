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
            var blog = FileStorage.LoadBlog();
        }
    }
}