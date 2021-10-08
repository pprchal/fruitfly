// Pavel Prchal, 2019, 2020

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly.core
{
    // Filesystem storage
    public class FileStorage : AbstractLogic, IStorage
    {
        public string LoadTemplate(string templateName) =>
            File.ReadAllText(GetFullTemplateName(templateName));
            
        private string TEMPLATES_ROOT =>
            Path.Combine(Context.Config.templateDir, Constants.Templates.Directory);

        private string BLOG_INPUT_ROOT =>
            Path.Combine(Context.Config.workDir, Constants.Blog.InputDirectory);

        private string BLOG_OUTPUT_ROOT =>
            Path.Combine(Context.Config.workDir);

        private string GetFullTemplateName(string templateName) =>
            Path.Combine(TEMPLATES_ROOT, Context.Config.template, templateName);

        public void WriteContent(IList<string> folderStack, string name, string content) =>
            File.WriteAllText(
                CreateFullPath(folderStack, name),
                content
            );

        public Blog Scan() =>
            Scan(BLOG_INPUT_ROOT);

        private Blog Scan(string rootDir)
        {
            var blog = new Blog();
            blog.Posts = (from directory in Directory.EnumerateDirectories(rootDir, "*.*", SearchOption.AllDirectories)
                     where directory != null
                     select TryParsePost(blog, directory))
                .OfType<Post>(); // skip nulls ;)
            return blog;
        }

        private string CreateFullPath(IList<string> folderStack, string name)
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

        public string LoadContentByStorageId(string storageId) =>
            File.ReadAllText(storageId);

        private static readonly Regex POST_PATTERN =
            new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        private static Post TryParsePost(Blog blog, string contentDir)
        {
            var m = POST_PATTERN.Match(contentDir);
            if(m.Success)
            {
                return (from mdFileInfo in new DirectoryInfo(contentDir).EnumerateFiles("*.md")
                        where IsTemplateContentFile(mdFileInfo)
                        select CreatePost(blog, m, mdFileInfo))
                .First();
            }

            return null;
        }

        private static Post CreatePost(Blog blog, Match m, FileInfo mdFileInfo) =>
            new Post(blog)
            {
                Name = mdFileInfo.Name,
                Title = mdFileInfo.Name.Substring(0, mdFileInfo.Name.Length - ".md".Length),
                StorageId = mdFileInfo.FullName,
                Created = new DateTime(
                    Convert.ToInt32(m.Groups[1].Value),
                    Convert.ToInt32(m.Groups[2].Value),
                    Convert.ToInt32(m.Groups[3].Value)
                ),
                Number = Convert.ToInt32(m.Groups[4].Value)
            };

        private static bool IsTemplateContentFile(FileInfo fileInfo) =>
            fileInfo.FullName.EndsWith(".md");
    }
}