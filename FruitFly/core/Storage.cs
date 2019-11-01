// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

        public void WriteContent(List<string> folderStack, string name, RenderedFormats format, string content)
        {
            File.WriteAllText(
                CreateFullPath(folderStack, name, format),
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
                var post = TryParsePost(Context, blog, directory);
                if(post != null)
                {
                    System.Console.Out.WriteLine($"\t~o~ {directory}");
                    blog.Posts.Add(post);
                }
            }

            return blog;
        }

        private string CreateFullPath(List<string> folderStack, string name, RenderedFormats format)
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
            
            return Path.Combine(outDirName, $"{name}.{format.ToString()}");
        }

        public string LoadByStorageId(string storageId)
        {
            return System.IO.File.ReadAllText(storageId);
        }

        private static Regex TemplateRe => new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        private static Post TryParsePost(Context context, Blog blog, string contentDir)
        {

            var m = TemplateRe.Match(contentDir);
            if(m.Success)
            {
                var dir = new DirectoryInfo(contentDir);
                foreach(var fileInfo in dir.EnumerateFiles("*.md"))
                {
                    if(IsTemplateContentFile(fileInfo))
                    {
                        return new Post(context, blog)
                        {
                            Name = fileInfo.Name,
                            Title = fileInfo.Name.Substring(0, fileInfo.Name.Length - ".md".Length),
                            StorageId = fileInfo.FullName,
                            Created = new DateTime(
                                Convert.ToInt32(m.Groups[1].Value), 
                                Convert.ToInt32(m.Groups[2].Value), 
                                Convert.ToInt32(m.Groups[3].Value)
                            ),
                            Number = Convert.ToInt32(m.Groups[4].Value)
                        };
                    }
                }
            }

            return null;
        }     

        private static bool IsTemplateContentFile(FileInfo fileInfo)
        {
            return fileInfo.FullName.EndsWith(".md");
        }      
    }
}