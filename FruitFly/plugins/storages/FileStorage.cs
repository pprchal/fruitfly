// Pavel Prchal, 2019

using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly.core
{
    // Filesystem storage
    class FileStorage : AbstractLogic, IStorage
    {
        string IStorage.LoadTemplate(string templateName) =>
            File.ReadAllText(GetFullTemplateName(templateName));

        string TEMPLATES_ROOT =>
            Path.Combine(Context.Config.TemplateDir, Constants.Templates.FOLDER);

        string BLOG_INPUT_ROOT =>
            Path.Combine(Context.Config.WorkDir, Constants.BLOG_INPUT);

        string BLOG_OUTPUT_ROOT =>
            Path.Combine(Context.Config.WorkDir);

        string GetFullTemplateName(string templateName) =>
            Path.Combine(TEMPLATES_ROOT, Context.Config.Template, templateName);

        void IStorage.WriteContent(string[] folderStack, string name, string content) =>
            File.WriteAllText(
                CreateFullPath(folderStack, name),
                content
            );

        Blog IStorage.Scan() => Scan(BLOG_INPUT_ROOT);

        Blog Scan(string rootDir) 
        {
            var blog = new Blog(this);
            blog.Posts = Directory
                .EnumerateDirectories(
                    rootDir, 
                    "*.*", 
                    SearchOption.AllDirectories
                )
                .Select(directory => TryParsePost(blog, directory))
                .Where(post => post != null);
            return blog;
        }

        string CreateFullPath(string[] folderStack, string name)
        {
            var outDirName = Path.Combine(
                BLOG_OUTPUT_ROOT,
                Path.Combine(folderStack)
            );

            if(!Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }
            
            return Path.Combine(outDirName, $"{name}");
        }

        string IStorage.LoadContentByStorageId(string storageId) =>
            File.ReadAllText(storageId);

        static readonly Regex TemplateRe =
            new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        Post TryParsePost(Blog blog, string contentDir)
        {
            var m = TemplateRe.Match(contentDir);
            if(!m.Success)
            {
                return null;
            }

            //  System.Console.Out.WriteLine($"\t~o~ {dir.FullName}");
            new DirectoryInfo(contentDir)
                .EnumerateFiles("*.md")
                .Where(fi => IsContentFile(fi))
                .Select(fileInfo => new Post(blog, this)
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
                    });

            return null;
        }     

        static bool IsContentFile(FileInfo fileInfo) =>
            fileInfo.FullName.EndsWith(".md");
    }
}