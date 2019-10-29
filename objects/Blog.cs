using System.Collections.Generic;

namespace fruitfly.objects
{
    public class Blog : HtmlContentObject
    {
        public Blog(Context context) : base(context)
        {
        }

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string Html => Context.Renderer.RenderBlog(this);
    }
}