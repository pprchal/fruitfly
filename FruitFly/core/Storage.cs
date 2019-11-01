// Pavel Prchal, 2019

using System.Collections.Generic;
using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    // Filesystem storage
    public class Storage : BaseLogic, IStorage
    {
        public string LoadTemplate(string templateName)
        {
            return File.ReadAllText(
                Path.Combine(Context.Config.rootDir, Global.TEMPLATES, Context.Config.template, templateName));
        }

        public void WriteContent(List<string> folderStack, string name, string content)
        {
            File.WriteAllText(
                CreateFullPath(folderStack, name),
                content
            );
        }

        public Blog Scan()
        {
            return Scan(Context.Config.rootDir == null ?
                Global.BLOG_INPUT :
                Path.Combine(Context.Config.rootDir, Global.BLOG_INPUT)
            );
        }

        private Blog Scan(string rootDir)
        {
            var blog = new Blog(Context, null);

            foreach(var directory in Directory.EnumerateDirectories(rootDir, "*.*", SearchOption.AllDirectories))
            {
                var post = Post.TryParse(Context, blog, directory);
                if(post != null)
                {
                    System.Console.Out.WriteLine($"\t~o~ {directory}");
                    blog.Posts.Add(post);
                }
            }

            return blog;
        }

        private string CreateFullPath(List<string> folderStack, string name)
        {
            var outDirName = Path.Combine(
                Context.Config.rootDir,
                Global.BLOG_OUTPUT,
                Path.Combine(folderStack.ToArray())
            );

            if(Context.Config.rootDir != "" && !Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }
            
            return Path.Combine(outDirName, name);
        }
    }
}