// Pavel Prchal, 2019

using System.Threading.Tasks;

namespace fruitfly
{
    public class BlogGenerator 
    {
        IStorage Storage => Runtime.Get<IStorage>();
        IConfiguration Configuration => Runtime.Get<IConfiguration>();

        public async Task<Blog> GenerateBlogAsync()
        {
            await System.Console.Out.WriteLineAsync($"FRUITFLY {Configuration.fullVersion} Blog generator");
            var blog = await Storage.LoadBlog();
            await GenerateBlogPostsFiles(blog);
            await GenerateBlogIndexFile(blog);
            return blog;
        }

        async Task GenerateBlogIndexFile(Blog blog) =>
            await Storage.WriteContent(
                folderStack: blog.BuildStoragePath(),
                name: Constants.Templates.INDEX, 
                content: await blog.Render()
            );

        async Task GenerateBlogPostsFiles(Blog blog) 
        {
            foreach(var post in blog.Posts)
            {
                var postContent = await post.Render();
                await Storage.WriteContent(
                    folderStack: post.BuildStoragePath(),
                    name: post.Name + ".html",
                    content: postContent
                );
            }
        }
    }
}             