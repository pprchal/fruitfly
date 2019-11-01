// Pavel Prchal, 2019

using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogScanner : BaseLogic
    {
        public Blog Scan()
        {
            return Scan(Context.Config.rootDir == null ?
                Global.BLOG_INPUT :
                Path.Combine(Context.Config.rootDir, Global.BLOG_INPUT)
            );
        }

        private Blog Scan(string rootDir)
        {
            var blog = new Blog(Context, null);

            foreach(var directory in Directory.EnumerateDirectories(rootDir, "*.*", SearchOption.AllDirectories))
            {
                var post = Post.TryParse(Context, blog, directory);
                if(post != null)
                {
                    System.Console.Out.WriteLine($"\t~o~ {directory}");
                    blog.Posts.Add(post);
                }
            }

            return blog;
        }
    }
}