// Pavel Prchal, 2019

using System;
using System.Linq;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogGenerator : AbstractLogic, IBlogGenerator
    {
        IStorage Storage;
        IConsole Console;
        IConverter Converter;

        public BlogGenerator(IStorage storage, IConsole console, IConverter converter)
        {
            Storage = storage; 
            Console = console;
            Converter = converter;
        }

        Blog IBlogGenerator.GenerateBlog(string[] args)
        {
            Console.WriteLine("~o~ FRUITFLY 1.0 Blog generator");
            var blog = Storage.Scan();
            GenerateBlogPostsFiles(blog);
            GenerateBlogIndexFile(blog);
            var seconds = new TimeSpan(DateTime.Now.Ticks - Context.StartTime.Ticks).TotalSeconds;
            Console.WriteLine($"{blog.Posts.Count()} ~o~ generated at: ${seconds} second(s)");
            return blog;
        }

        void GenerateBlogIndexFile(Blog blog) =>
            Storage.WriteContent(
                folderStack: blog.BuildStoragePath().ToArray(),
                name: Constants.Templates.INDEX, 
                content: blog.Render(Converter)
            );

        void GenerateBlogPostsFiles(Blog blog) 
        {
            foreach(var post in blog.Posts)
            {
                Storage.WriteContent(
                    folderStack: post.BuildStoragePath().ToArray(),
                    name: post.Name + ".html",
                    content: post.Render(Converter)
                );
            }
        }
    }
}             