// Pavel Prchal, 2019

using System;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogGenerator : BaseLogic
    {
        public Blog GenerateBlog(string[] args)
        {
            Context.Console.WriteLine("~o~ FRUITFLY 1.0 Blog generator");

            var blog = Context.GetLogic<BlogScanner>().Scan();
            GenerateBlogPostsFiles(blog);
            GenerateBlogIndexFile(blog);

            var seconds = new TimeSpan(DateTime.Now.Ticks - Context.StartTime.Ticks).TotalSeconds;
            Context.Console.WriteLine($"{blog.Posts.Count} ~o~ generated at: ${seconds} second(s)");

            return blog;
        }

        private void GenerateBlogIndexFile(Blog blog)
        {
            Context.GetLogic<Storage>().WriteContent(
                template: Templates.Index, 
                content: Context.GetLogic<HtmlRenderer>().Render(blog)
            );
        }

        private void GenerateBlogPostsFiles(Blog blog)
        {
            foreach(var post in blog.Posts)
            {
                Context.GetLogic<Storage>().WriteContent(
                    template: Templates.Post, 
                    content: Context.GetLogic<HtmlRenderer>().Render(post), 
                    post: post
                );
            }
        }
    }
}