// Pavel Prchal, 2019

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Post : AbstractContentObject
    {

        public Post(Context context, AbstractContentObject parent) : base(context, parent)
        {
        }

        public string Name
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string TitleTile
        {
            get
            {
                return Title;
            }
        }

        public DateTime Created
        {
            get;
            set;
        }

        public string StorageId
        {
            get;
            set;
        }

        public int Number;

        private static Regex TemplateRe => new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        public static Post TryParse(Context context, Blog blog, string contentDir)
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

        public string MdContent
        {
            get
            {
                return Context.GetLogic<Storage>().LoadByStorageId(StorageId);
            }
        }


        public string Url 
        { 
            get
            {
                var urlFolderStack = BlogGenerator.BuildFolderStack(this);
                urlFolderStack.Add(Name + ".html");
                var x = string.Join("\\", urlFolderStack);
                return x;
                // return Directory.Parent.Parent.Name + "\\" + Directory.Parent.Name + "\\" + Directory.Name + "\\" + ;
            }
        }
        
        public override string GetVariableValue(Variable variable) => variable switch
        {
            { Scope: "post", Name: Global.VAR_NAME_POST_TITLE }  => Title,
            { Scope: "post", Name: Global.VAR_NAME_POST_TITLE_TILE }  => TitleTile,
            { Scope: "post", Name: Global.VAR_NAME_POST_CREATED }  => ToLocaleDate(Created),
            { Scope: "post", Name: Global.VAR_NAME_POST_URL } => Url,
            { Scope: "post", Name: Global.VAR_NAME_POST_CONTENT }  => MdConverter.Convert(MdContent),
            _ => Parent.GetVariableValue(variable)
        };


        private CultureInfo _Culture = null;

        CultureInfo Culture
        {
            get
            {
                if (_Culture == null)
                {
                    _Culture = new CultureInfo(Context.Config.language.Replace("_", "-"));
                }
                return _Culture;
            }
        }

        private string ToLocaleDate(DateTime date)
        {
            return date.ToString("d", Culture);
        }

        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();
    }
}