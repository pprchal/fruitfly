// Pavel Prchal, 2019, 2020

using System;
using System.Linq;
using System.Threading.Tasks;
using fruitfly.objects;

namespace fruitfly.core
{
    public static class BlogGenerator 
    {
        public static Blog GenerateBlog()
        {
            Context.ConsoleWrite("~o~ FRUITFLY 2.0 Blog generator");
            
            
            var blog = Context.Storage.Scan();



            GeneratePosts(blog);
            GenerateBlogIndexFile(blog);
            
            Context.ConsoleWrite($"{blog.Posts.Count()} ~o~ generated at: ${blog.EllapsedSeconds} second(s)");
            return blog;
        }

        private static void GenerateBlogIndexFile(Blog blog) =>
            Context.Storage.WriteContent(
                folderStack: blog,
                name: Constants.Templates.Index, 
                content: blog.Render(RenderedFormats.Html)
            );

        private static async void GeneratePosts(Blog blog) 
        {
            foreach (var post in blog.Posts)
            {
                await GeneratePost(post);
            }
        }

        private static async Task GeneratePost(Post post) =>
            await Context
                .Storage
                .WriteContent(
                    folderStack: post,
                    name: post.Name + ".html",
                    content: post.Render(RenderedFormats.Html)
                );
    }
}             