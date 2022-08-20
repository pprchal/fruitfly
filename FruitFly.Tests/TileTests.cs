using NUnit.Framework;
using fruitfly.core;
using fruitfly.objects;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void IsTileRenderingOK()
        {
            var config = new Configuration();

            var storage = new FileStorage(config, new fruitfly.Console()) as IStorage;
            var result = new Post(
                config,
                null, 
                storage
            ).Render(null, Constants.MORPH_TILE).Result;
        }
    }
}