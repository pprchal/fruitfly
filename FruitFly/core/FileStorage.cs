// Pavel Prchal, 2019, 2023

using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace fruitfly
{
    // Filesystem storage
    public static class FileStorage
    {
        public static async Task<string> LoadTemplate(string templateName) =>
            await ReadContentAsync(GetFullTemplateName(templateName));

        public static async Task<string> LoadContentByStorageId(string storagePath) =>
            await ReadContentAsync(storagePath);

        static Task<string> ReadContentAsync(string storagePath) =>
            File.ReadAllTextAsync(storagePath);

        public static async Task<bool> WriteContent(string[] folderStack, string name, string content) 
        {
            var fullFileName = CreateFullPath(folderStack, name);
            Runtime.WriteLine($"W {fullFileName}");
            await File.WriteAllTextAsync(fullFileName, content);
            return true;
        }

        static string BLOG_INPUT_ROOT =>
            Path.Combine(Runtime.Configuration.workDir, Constants.BLOG_INPUT);

        static string BLOG_OUTPUT_ROOT => Runtime.Configuration.workDir;

        static string GetFullTemplateName(string templateName) =>
            Path.Combine(
                Runtime.Configuration.templateDir, 
                Runtime.Configuration.template, 
                templateName
            );

        public static Task<Blog> LoadBlog() =>
            Task.Run(() => LoadBlogFiles(BLOG_INPUT_ROOT));

        static Blog LoadBlogFiles(string rootDir) 
        {
            if(!Directory.Exists(rootDir))
            {
                throw new DirectoryNotFoundException($"Directory not found: {rootDir}");
            }

            Runtime.WriteLine($"Scanning: {rootDir}");
            var blog = new Blog();
            foreach(var post in EnumeratePosts())
            {
                blog.AddPost(post);
            }
            return blog;
        }

        static IEnumerable<Post> EnumeratePosts() =>
            EnumerateMdFiles()
                .Select(fileInfo => new StorageItem(fileInfo, DirectoryRe.Match(fileInfo.DirectoryName))) // DirectoryRe.Match(fileInfo.DirectoryName);
                .Where(storageItem => storageItem.M.Success)
                .Select(CreatePost);

        static Post CreatePost(StorageItem storageItem) => new (
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

        static IEnumerable<FileInfo> EnumerateMdFiles() =>
            new DirectoryInfo(BLOG_INPUT_ROOT)
                .EnumerateFiles("*.md", new EnumerationOptions{ RecurseSubdirectories = true});

        internal readonly struct StorageItem
        {
            internal StorageItem(FileInfo fileInfo, Match match)
            {
                FileInfo = fileInfo;
                M = match;
            }

            public readonly FileInfo FileInfo;
            public readonly Match M;
        }

        static string CreateFullPath(string[] folderStack, string name)
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