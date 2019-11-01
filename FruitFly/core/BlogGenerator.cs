// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogGenerator : BaseLogic
    {
        public Blog GenerateBlog(string[] args)
        {
            Context.Console.WriteLine("~o~ FRUITFLY 1.0 Blog generator");
            var blog = Context.GetLogic<Storage>().Scan();
            GenerateBlogPostsFiles(blog);
            GenerateBlogIndexFile(blog);
            var seconds = new TimeSpan(DateTime.Now.Ticks - Context.StartTime.Ticks).TotalSeconds;
            Context.Console.WriteLine($"{blog.Posts.Count} ~o~ generated at: ${seconds} second(s)");
            return blog;
        }

        private void GenerateBlogIndexFile(Blog blog)
        {
            Context.GetLogic<Storage>().WriteContent(
                folderStack: BuildFolderStack(blog),
                name: Global.TEMPLATE_INDEX, 
                content: Context.GetLogic<HtmlRenderer>().RenderTemplate(Global.TEMPLATE_INDEX, blog)
            );
        }

        private void GenerateBlogPostsFiles(Blog blog)
        {
            foreach(var post in blog.Posts)
            {
                Context.GetLogic<Storage>().WriteContent(
                    folderStack: BuildFolderStack(post),
                    name: post.File.Name + ".html", 
                    content: Context.GetLogic<HtmlRenderer>().RenderTemplate(Global.TEMPLATE_POST, post)
                );
            }
        }

        private List<string> BuildFolderStack(AbstractContentObject contentObject)
        {
            if(contentObject is Blog)
            {
                return new List<string>();
            }

            var post = contentObject as Post;
            return new List<string>()
            {
                $"y{post.Created.Year}",
                $"m{post.Created.Month}",
                $"d{post.Created.Day}_post{post.Number}"
            };
        }
    }
}