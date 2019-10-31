// Pavel Prchal, 2019

using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogScanner : BaseLogic
    {
        public Blog Scan(string rootDir)
        {
            var blog = new Blog();
            foreach(var directory in Directory.EnumerateDirectories(Global.BLOG_INPUT, "*.*", SearchOption.AllDirectories))
            {
                var post = Post.TryParse(directory);
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