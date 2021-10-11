// Pavel Prchal, 2019

using System.Linq;
using System.Threading.Tasks;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogGenerator : IBlogGenerator
    {
        readonly IStorage Storage;
        readonly IConsole Console;
        readonly IConverter Converter;

        public BlogGenerator(IStorage storage, IConsole console, IConverter converter)
        {
            Storage = storage; 
            Console = console;
            Converter = converter;
        }

        Task<Blog> IBlogGenerator.GenerateBlogAsync() =>
            Task.Run(async () =>
            {
                Console.WriteLine($"~o~ FRUITFLY {Context.Config.fullVersion} Blog generator");
                var blog = await Storage.Scan();
                await GenerateBlogPostsFiles(blog);
                await GenerateBlogIndexFile(blog);
                return blog;
            });

        async Task GenerateBlogIndexFile(Blog blog) =>
            await Storage.WriteContent(
                folderStack: blog.BuildStoragePath(),
                name: Constants.Templates.INDEX, 
                content: await blog.Render(Converter)
            );

        async Task GenerateBlogPostsFiles(Blog blog) 
        {
            foreach(var post in blog.Posts)
            {
                var postContent = await post.Render(Converter);
                await Storage.WriteContent(
                    folderStack: post.BuildStoragePath(),
                    name: post.Name + ".html",
                    content: postContent
                );
            }
        }
    }
}             