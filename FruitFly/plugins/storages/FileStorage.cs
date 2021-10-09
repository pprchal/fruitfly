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
            ReadContent(GetFullTemplateName(templateName));

        string IStorage.LoadContentByStorageId(string storagePath) =>
            ReadContent(storagePath);


        string ReadContent(string storagePath)
        {
            Console.WriteLine($"R< {storagePath}");
            return File.ReadAllText(storagePath);
        }

        void IStorage.WriteContent(string[] folderStack, string name, string content) 
        {
            var fullFileName = CreateFullPath(folderStack, name);
            Console.WriteLine($"W> {fullFileName}");
            File.WriteAllText(fullFileName, content);
        }

        string TEMPLATES_ROOT =>
            Path.Combine(Context.Config.templateDir, Constants.Templates.FOLDER);

        string BLOG_INPUT_ROOT =>
            Path.Combine(Context.Config.workDir, Constants.BLOG_INPUT);

        string BLOG_OUTPUT_ROOT => Context.Config.workDir;

        string GetFullTemplateName(string templateName) =>
            Path.Combine(TEMPLATES_ROOT, Context.Config.template, templateName);



        Blog IStorage.Scan() => Scan(BLOG_INPUT_ROOT);

        static readonly Regex YEAR = new Regex("y(\\d+)");
        Blog Scan(string rootDir) 
        {
            var blog = new Blog(this);

            blog.Posts = new DirectoryInfo(rootDir)
                .EnumerateFiles("*.md", new EnumerationOptions{ RecurseSubdirectories = true})
                .Select(fileInfo => (FileInfo: fileInfo, M: DirectoryRe.Match(fileInfo.DirectoryName)))
                .Where(t => t.M.Success)
                .Select(t => new Post(blog, this)
                {
                    Name = t.FileInfo.Name,
                    Title = t.FileInfo.Name.Substring(0, t.FileInfo.Name.Length - ".md".Length),
                    StorageId = t.FileInfo.FullName,
                    Created = new DateTime(
                        Convert.ToInt32(t.M.Groups[1].Value), 
                        Convert.ToInt32(t.M.Groups[2].Value), 
                        Convert.ToInt32(t.M.Groups[3].Value)
                    ),
                    Number = Convert.ToInt32(t.M.Groups[4].Value)
                });

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



        // static readonly Regex DirectoryRe =
        //     new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        static readonly Regex DirectoryRe =
            new Regex(@"y(\d+)\/m(\d+)\/d([\d+]+)_post([\d+]+$)", RegexOptions.Compiled);
    }
}