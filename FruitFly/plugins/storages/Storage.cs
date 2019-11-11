// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly.core
{
    // Filesystem storage
    public class Storage : AbstractLogic, IStorage
    {
        public string LoadTemplate(string templateName)
        {
            return File.ReadAllText(GetFullTemplateName(templateName));
        }

        private string TEMPLATES_ROOT
        {
            get
            {
                return Path.Combine(Context.Config.templateDir, Global.TEMPLATES);
            }
        }

        private string BLOG_INPUT_ROOT
        {
            get
            {
                return Path.Combine(Context.Config.workDir, Global.BLOG_INPUT);
            }
        }

        private string BLOG_OUTPUT_ROOT
        {
            get
            {
                return Path.Combine(Context.Config.workDir);
            }
        }

        private string GetFullTemplateName(string templateName)
        {
            return Path.Combine(TEMPLATES_ROOT, Context.Config.template, templateName);
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
            return Scan(BLOG_INPUT_ROOT);
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

        private string CreateFullPath(List<string> folderStack, string name)
        {
            var outDirName = Path.Combine(
                BLOG_OUTPUT_ROOT,
                Path.Combine(folderStack.ToArray())
            );

            if(!Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }
            
            return Path.Combine(outDirName, $"{name}");
        }

        public string LoadContentByStorageId(string storageId)
        {
            return File.ReadAllText(storageId);
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