using NUnit.Framework;
using fruitfly.core;

namespace FruitFly.Tests
{
    [TestFixture]
    public class HtmlRenderedTests 
    {
        Context ctx = new Context();

        [Test]
        public void CanRenderPostTile()
        {
            var blog = ctx.GetLogic<Storage>().Scan();
            var html = ctx.GetLogic<HtmlRenderer>().RenderTemplate(
                Global.TEMPLATE_POST,
                blog.Posts[0]
            ).ToString();
        }
    }
}
