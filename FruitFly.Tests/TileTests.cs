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
            var storage = new FileStorage(new fruitfly.Console()) as IStorage;
            var result = new Post(
                null, 
                storage
            ).Render(null, Constants.MORPH_TILE).Result;
        }
    }
}