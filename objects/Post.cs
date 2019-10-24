using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace fruitfly.objects
{
    public class Post
    {
        public PostHeader Header
        public PostHeader Header
        {
            get
            {

            }
        }
        public DateTime Created
        {
            get;
            set;
        }

        public string Article
        {
            get;
            set;
        } = "ddddddddddddddddd";

        public DirectoryInfo Directory
        {
            get;
            set;
        }

        public int Day;
        public int Number;
        private static Regex TemplateRe = new Regex("d([0-9]+)_post([0-9]+$)", RegexOptions.Compiled);

        public string Name { get; internal set; }

        public static Post TryParse(string contentDir)
        {
            var m = TemplateRe.Match(contentDir);
            if(m.Success)
            {
                return new Post()
                {
                    Name = contentDir,
                    Day = Convert.ToInt32(m.Groups[1].Value),
                    Number = Convert.ToInt32(m.Groups[2].Value),
                    Directory = new DirectoryInfo(contentDir)
                };
            }

            return null;
        }    

        private bool IsTemplateContentFile(FileInfo fileInfo)
        {
            return fileInfo.FullName.EndsWith(".md");
        }    

        private FileInfo _ArticleFileInfo = null;
        public FileInfo ArticleFileInfo
        {
            get
            {
                if(_ArticleFileInfo == null)
                {
                    foreach(var fileInfo in Directory.EnumerateFiles("*.md"))
                    {
                        if(IsTemplateContentFile(fileInfo))
                        {
                            _ArticleFileInfo = fileInfo;
                        }
                    }
                }

                return _ArticleFileInfo;
            }
        }  
    }
}