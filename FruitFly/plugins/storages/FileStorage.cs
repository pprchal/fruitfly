// Pavel Prchal, 2019

using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly.core
{
    // Filesystem storage
    public class FileStorage : IStorage
    {
        IConsole Console;

        public FileStorage(IConsole console)
        {
            Console = console;
        }

        string IStorage.LoadTemplate(string templateName) =>
            File.ReadAllText(GetFullTemplateName(templateName));

        string TEMPLATES_ROOT =>
            Path.Combine(Context.Config.templateDir, Constants.Templates.FOLDER);

        string BLOG_INPUT_ROOT =>
            Path.Combine(Context.Config.workDir, Constants.BLOG_INPUT);

        string BLOG_OUTPUT_ROOT => Context.Config.workDir;

        string GetFullTemplateName(string templateName) =>
            Path.Combine(TEMPLATES_ROOT, Context.Config.template, templateName);

        void IStorage.WriteContent(string[] folderStack, string name, string content) =>
            File.WriteAllText(
                CreateFullPath(folderStack, name),
                content
            );

        Blog IStorage.Scan() => Scan(BLOG_INPUT_ROOT);

        Blog Scan(string rootDir) 
        {
            var blog = new Blog(this);

            var candidateDirs = new DirectoryInfo(rootDir).EnumerateDirectories(                
                searchPattern: "*",
                new EnumerationOptions{ RecurseSubdirectories = true }
            );

            blog.Posts = candidateDirs
                .Select(candidateDir => TryParsePost(blog, candidateDir.FullName))
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

        // static readonly Regex DirectoryRe =
        //     new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        static readonly Regex DirectoryRe =
            new Regex(@"y(\d+)\/m(\d+)\/d([\d+]+)_post([\d+]+$)", RegexOptions.Compiled);
            

        Post TryParsePost(Blog blog, string candidateDir)
        {
            var m = DirectoryRe.Match(candidateDir);
            if(!m.Success)
            {
                return null;
            }

            Console.WriteLine($"\t~o~ {candidateDir}");
            return new DirectoryInfo(candidateDir)
                .EnumerateFiles()
                .Where(fi => IsContentFile(fi))
                .Select(fileInfo => 
                    new Post(blog, this)
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
                    })
                .FirstOrDefault();
        }     

        static bool IsContentFile(FileInfo fileInfo) =>
            fileInfo.FullName.EndsWith(".md");
    }
}