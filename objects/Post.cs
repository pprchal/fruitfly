using System;
using System.IO;
using System.Text.RegularExpressions;

namespace fruitfly.objects
{
    public class Post : HtmlContentObject, IVariableProvider
    {
        public Post(IVariableProvider parent) : base(parent)
        {
        }

        public FileInfo Info
        {
            get;
        }

        public string Title
        {
            get
            {
                return Info.Name;
            }
        }

        public DateTime Created
        {
            get
            {
                return Info.CreationTime;
            }
        }

        public override string Html => HtmlRenderer.RenderPost(this);

     
        public DirectoryInfo Directory
        {
            get;
            set;
        }

        public int Day;
        public int Number;
        private static Regex TemplateRe = new Regex("d([0-9]+)_post([0-9]+$)", RegexOptions.Compiled);

        public string Name { get; internal set; }

        public static Post TryParse(Blog parentBlog, string contentDir)
        {
            var m = TemplateRe.Match(contentDir);
            if(m.Success)
            {
                return new Post(parentBlog)
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

        string IVariableProvider.GetVariableValue(string name)
        {
            // {post:title}
            // {post:created}
            if(name == "post:title")
            {
                return Title;
            }
            else if(name == "post:created")
            {
                return HtmlRenderer.RenderDate(Created);
            }

            return Parent.GetVariableValue(name); 
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