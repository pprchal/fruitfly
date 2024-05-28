// Pavel Prchal, 2019, 2023

using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace fruitfly.plugins
{
    // Filesystem storage
    public class FileStorage : IStorage
    {
        IConsole Console => Runtime.Get<IConsole>();
        IConfiguration Configuration => Runtime.Get<IConfiguration>();

        async Task<string> IStorage.LoadTemplate(string templateName) =>
            await ReadContentAsync(GetFullTemplateName(templateName));

        async Task<string> IStorage.LoadContentByStorageId(string storagePath) =>
            await ReadContentAsync(storagePath);

        Task<string> ReadContentAsync(string storagePath) =>
            File.ReadAllTextAsync(storagePath);

        Task IStorage.WriteContent(string[] folderStack, string name, string content) 
        {
            var fullFileName = CreateFullPath(folderStack, name);
            Console.WriteLine($"W {fullFileName}");
            return File.WriteAllTextAsync(fullFileName, content);
        }

        string BLOG_INPUT_ROOT =>
            Path.Combine(Configuration.workDir, Constants.BLOG_INPUT);

        string BLOG_OUTPUT_ROOT => Configuration.workDir;

        string GetFullTemplateName(string templateName) =>
            Path.Combine(
                Configuration.templateDir, 
                Configuration.template, 
                templateName
            );

        Task<Blog> IStorage.LoadBlog() =>
            Task.Run(() => LoadBlogFiles(BLOG_INPUT_ROOT));

        Blog LoadBlogFiles(string rootDir) 
        {
            if(!Directory.Exists(rootDir))
            {
                return Blog.Error($"Directory not found: {rootDir}");
            }

            Console.WriteLine($"Scanning: {rootDir}");
            var blog = new Blog();
            foreach(var post in EnumeratePosts())
            {
                blog.AddPost(post);
            }
            return blog;
        }

        IEnumerable<Post> EnumeratePosts() =>
            EnumerateMdFiles()
                .Select(fileInfo => new StorageItem(fileInfo, DirectoryRe.Match(fileInfo.DirectoryName))) // DirectoryRe.Match(fileInfo.DirectoryName);
                .Where(storageItem => storageItem.M.Success)
                .Select(CreatePost);

        Post CreatePost(StorageItem storageItem) => new (
            name: storageItem.FileInfo.Name,
            title: storageItem.FileInfo.Name[..^".md".Length],
            storageId: storageItem.FileInfo.FullName,
            created: new DateTime(
                Convert.ToInt32(storageItem.M.Groups[1].Value), 
                Convert.ToInt32(storageItem.M.Groups[2].Value), 
                Convert.ToInt32(storageItem.M.Groups[3].Value)
            ),
            number: Convert.ToInt32(storageItem.M.Groups[4].Value)
        );

        IEnumerable<FileInfo> EnumerateMdFiles() =>
            new DirectoryInfo(BLOG_INPUT_ROOT)
                .EnumerateFiles("*.md", new EnumerationOptions{ RecurseSubdirectories = true});

        internal sealed class StorageItem
        {
            internal StorageItem(FileInfo fileInfo, Match m)
            {
                FileInfo = fileInfo;
                M = m;
            }

            public FileInfo FileInfo;
            public Match M;
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

        static readonly Regex DirectoryRe =         
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            RExps.DIRECTORY_WIN
            :
            RExps.DIRECTORY_UNIX;
    }
}