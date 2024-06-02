// Pavel Prchal, 2019

using System.Threading.Tasks;

namespace fruitfly
{
    public class BlogGenerator 
    {
        public static async Task<bool> GenerateBlogAsync()
        {
            Runtime.WriteLine($"FRUITFLY {Runtime.Configuration.fullVersion} blog generator");
            var blog = await FileStorage.LoadBlog();
            var posts = await GeneratePosts(blog);
            Runtime.WriteLine($"Generated posts: {posts}");
            return await GenerateIndexHtml(blog);
        }

        static async Task<bool> GenerateIndexHtml(Blog blog) =>
            await FileStorage.WriteContent(
                folderStack: blog.BuildStoragePath(),
                name: Constants.Templates.INDEX, 
                content: await blog.Render()
            );

        static async Task<int> GeneratePosts(Blog blog) 
        {
            var n = 0;
            foreach(var post in blog.Posts)
            {
                var postContent = await post.Render();
                var saved = await FileStorage.WriteContent(
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