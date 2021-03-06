// Pavel Prchal, 2019

using System;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogGenerator : AbstractLogic
    {
        public Blog GenerateBlog(string[] args)
        {
            Context.Current.Console.WriteLine("~o~ FRUITFLY 1.0 Blog generator");
            var blog = Context.GetLogic<Storage>().Scan();
            GenerateBlogPostsFiles(blog);
            GenerateBlogIndexFile(blog);
            var seconds = new TimeSpan(DateTime.Now.Ticks - Context.Current.StartTime.Ticks).TotalSeconds;
            Context.Current.Console.WriteLine($"{blog.Posts.Count} ~o~ generated at: ${seconds} second(s)");
            return blog;
        }

        private void GenerateBlogIndexFile(Blog blog) =>
            Context.GetLogic<Storage>().WriteContent(
                folderStack: blog.BuildFolderStack(),
                name: Global.TEMPLATE_INDEX, 
                content: blog.Render(RenderedFormats.Html)
            );

        private void GenerateBlogPostsFiles(Blog blog) =>
            blog
            .Posts
            .ForEach(post =>
            {
                Context.GetLogic<Storage>().WriteContent(
                    folderStack: post.BuildFolderStack(),
                    name: post.Name + ".html",
                    content: post.Render(RenderedFormats.Html)
                );
            });
    }
}             