// Pavel Prchal, 2019

using System.Threading.Tasks;

namespace fruitfly
{
    public class BlogGenerator 
    {
        public async Task<Blog> GenerateBlogAsync()
        {
            Runtime.Console.WriteLine($"FRUITFLY {Runtime.Configuration.fullVersion} Blog generator");
            var blog = await Runtime.Storage.LoadBlog();

            var blogs = await GeneratePosts(blog);
            Runtime.Console.WriteLine($"Generated posts: {blogs}");

            await GenerateIndexHtml(blog);
            return blog;
        }

        async Task<bool> GenerateIndexHtml(Blog blog) =>
            await Runtime.Storage.WriteContent(
                folderStack: blog.BuildStoragePath(),
                name: Constants.Templates.INDEX, 
                content: await blog.Render()
            );

        async Task<int> GeneratePosts(Blog blog) 
        {
            var n = 0;
            foreach(var post in blog.Posts)
            {
                var postContent = await post.Render();
                var saved = await Runtime.Storage.WriteContent(
                    folderStack: post.BuildStoragePath(),
                    name: $"{post.Name}.html",
                    content: postContent
                );
                n++;
            }

            return n;
        }
    }
}             