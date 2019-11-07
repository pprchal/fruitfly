using NUnit.Framework;
using fruitfly.core;

namespace FruitFly.Tests
{
    [TestFixture]
    public class TemplateRendererTests 
    {
        Context ctx = new Context();

        [Test]
        public void CanRenderPostTile()
        {
            var blog = ctx.GetLogic<Storage>().Scan();
            var html = blog.Posts[0].Render(RenderedFormats.Html).ToString();
        }
    }
}
