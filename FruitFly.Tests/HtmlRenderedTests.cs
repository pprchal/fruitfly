using NUnit.Framework;
using fruitfly.core;
using fruitfly.objects;

namespace FruitFly.Tests
{
    [TestFixture]
    public class HtmlRenderedTests 
    {
        Context ctx = new Context();

        [Test]
        public void CanRenderPostTile()
        {
            var blog = ctx.GetLogic<BlogScanner>().Scan();
            var html = ctx.GetLogic<HtmlRenderer>().Render(
                blog.Posts[0],
                Templates.PostTile
            ).ToString();
        }
    }
}
