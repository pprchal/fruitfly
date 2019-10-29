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
            var blog = new Blog(context.Config);
            foreach(var directory in Directory.EnumerateDirectories(Global.BLOG_INPUT, "*.*", SearchOption.AllDirectories))
            {
                var post = Post.TryParse(blog, directory);
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



        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();


// <div class="jumbotron">
//   <h1 class="display-4">{post:title}</h1>
//   <p class="lead">{post:body}</p>
//   <hr class="my-4">
//   <p>It uses utility classes for typography and spacing to space content out within the larger container.</p>
//   <a class="btn btn-primary btn-lg" href="#" role="button">Learn more</a>
// </div>        
    }
}