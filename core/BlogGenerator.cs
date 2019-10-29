using System.IO;
using System.Text;
using fruitfly.objects;

namespace fruitfly
{
    public class BlogGenerator
    {
        public Context Context { get; }

        private BlogGenerator()
        {
        }

        public BlogGenerator(Context context)
        {
            Context = context;
        }

        public Blog GenerateBlog()
        {
            var blog = new BlogScanner(Context).Scan(Global.BLOG_INPUT);
            File.WriteAllText(Path.Combine(Global.BLOG_OUTPUT, Global.INDEX_HTML), Context.Renderer.RenderBlog(blog));
            RenderBlogPosts(blog);
            return blog;
        }

        private void RenderBlogPosts(Blog blog)
        {
            var sb = new StringBuilder();
            foreach(var post in blog.Posts)
            {
                sb.Append(Context.Renderer.RenderPostAsJumbotron(post));

                File.WriteAllText(
                    GetOutFileNameAndEnsureDir(post),
                    Context.Renderer.RenderPost(post)
                );
            }
        }

        private string GetOutFileNameAndEnsureDir(Post post)
        {
            var outDirName = post.Name.Replace(Global.BLOG_INPUT + "\\", Global.BLOG_OUTPUT + "\\");
            if(!Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }

            return Path.Combine(outDirName, post.ArticleFileInfo.Name + ".html");
        }
    }
}