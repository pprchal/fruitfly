using System;
using System.IO;
using System.Text.RegularExpressions;

namespace fruitfly.objects
{
    public class Post : HtmlContentObject
    {
        public Post(Context context) : base(context)
        {
        }

        public FileInfo File
        {
            get;
        }

        public string Title
        {
            get
            {
                return File.Name;
            }
        }

        public DateTime Created
        {
            get
            {
                return File.CreationTime;
            }
        }

        public override string Html => Context.Renderer.RenderPost(this);

     
        public DirectoryInfo Directory
        {
            get;
            set;
        }

        public int Day;
        public int Number;
        private static Regex TemplateRe = new Regex("d([0-9]+)_post([0-9]+$)", RegexOptions.Compiled);

        public string Name { get; internal set; }

        public static Post TryParse(Context context, string contentDir)
        {
            var m = TemplateRe.Match(contentDir);
            if(m.Success)
            {
                return new Post(context)
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


        // public virtual string GetVariableValue(string name)
        // {
        //     // {post:title}
        //     // {post:created}
        //     if(name == "post:title")
        //     {
        //         return Title;
        //     }
        //     else if(name == "post:created")
        //     {
        //         create
        //         // DateFormat f
        //         // return Created.to
        //     }

        //     return Parent.GetVariableValue(name);
        // }


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