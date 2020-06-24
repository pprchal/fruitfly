using NUnit.Framework;
using fruitfly.core;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TemplateRendererTests 
    {
        [Test]
        public void CanRenderPostTile()
        {
            var blog = Context.GetLogic<Storage>().Scan();
            var html = blog.Posts[0].Render(RenderedFormats.Html).ToString();
        }
    }
}
