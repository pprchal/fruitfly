using System.IO;
using fruitfly.objects;

namespace fruitfly
{
    public class BlogScanner
    {
        private BlogScanner()
        {

        }

        private Context context;

        public BlogScanner(Context context)
        {
            this.context = context;
        }

        public Blog Scan(string rootDir)
        {
            var blog = new Blog(context);
            foreach(var directory in Directory.EnumerateDirectories(Global.BLOG_INPUT, "*.*", SearchOption.AllDirectories))
            {
                var post = Post.TryParse(context, directory);
                if(post != null)
                {
                    System.Console.Out.WriteLine($"\t~o~ {directory}");
                    blog.Posts.Add(post);
                }
            }

            return blog;
        }

        private bool IsTemplateContentFile(FileInfo fileInfo)
        {
            return fileInfo.FullName.EndsWith(".md");
        }
    }
}