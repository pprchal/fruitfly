using NUnit.Framework;
using fruitfly.core;
using System.IO;
using System.Linq;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TemplateRendererTests : IVariableSource
    {
        [Test]
        public void Post_htmlTile()
        {
            var blog = Context.Storage.Scan();
            var html = blog.Posts.First().Render(RenderedFormats.Html).ToString();
        }

        [Test]
        public void Post_html()
        {
            var content = File.ReadAllText("templates\\default\\post.html");
            var result = TemplateProcessor.Process(content, this, "--test--");
        }

        string IVariableSource.GetVariableValue(Variable variable)
        {
            return "-";
        }

        [OneTimeSetUp]
        public void Init() =>
            TestFruitContext.Init();
    }
}
