using NUnit.Framework;
using fruitfly;
using fruitfly.plugins;
using System.Threading.Tasks;
using System;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TileTests : IVariableSource
    {
        [Test]
        public void IsTileRenderingOK()
        {
            var post = new Post(
                name: "My POST",
                title: "ěščř",
                storageId: "",
                created: DateTime.Now,
                number: 1
            );
            var result = post.Render(Constants.MORPH_TILE).Result;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            Runtime.Add<IConsole>(new fruitfly.Console());
            Runtime.Add<IConfiguration>(new YamlConfiguration());
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