using NUnit.Framework;
using fruitfly;
using fruitfly.plugins;
using System.Threading.Tasks;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TileTests : IVariableSource
    {
        [Test]
        public void IsTileRenderingOK()
        {
            var post = new Post(null);
            var result = post.Render(Constants.MORPH_TILE).Result;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            Runtime.Add<IConsole>(new Console());
            Runtime.Add<IConfiguration>(new Configuration());
            Runtime.Add<IVariableSource>(this);
            Runtime.Add<IConverter>(new MarkdigHtmlConverter());
            Runtime.Add<IStorage>(new FileStorage());
        }

        Task<string> IVariableSource.GetVariableValue(Variable variable)
        {
            return Task.FromResult("");
        }
    }
}