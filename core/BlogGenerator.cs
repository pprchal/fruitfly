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

            var blog = Context.GetLogic<BlogScanner>().Scan(Global.BLOG_INPUT);
            GenerateBlogPostsFiles(blog);
            GenerateBlogIndexFile(blog);

            var seconds = new TimeSpan(DateTime.Now.Ticks - Context.StartTime.Ticks).TotalSeconds;
            Context.Console.WriteLine($"{blog.Posts.Count} ~o~ generated at: ${seconds} second(s)");

            return blog;
        }

        private void GenerateBlogIndexFile(Blog blog)
        {
            Context.GetLogic<BlogStorage>().WriteContent(
                templateItem: TemplateItems.Index, 
                content: Context.GetLogic<HtmlRenderer>().RenderIndex(blog)
            );
        }

        private void GenerateBlogPostsFiles(Blog blog)
        {
            foreach(var post in blog.Posts)
            {
                Context.GetLogic<BlogStorage>().WriteContent(
                    templateItem: TemplateItems.Post, 
                    content: Context.GetLogic<HtmlRenderer>().RenderPost(post), 
                    post: post
                );
            }
        }
    }
}