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
        protected Context Context { get; }

        public Post(Context context, AbstractContentObject parent) : base(parent)
        {
            Context = context;
        }

        public string Title
        {
            get
            {
                return File.Name.Substring(0, File.Name.Length - ".md".Length);
            }
        }

        public string TitleTile
        {
            get
            {
                return File.Name;
            }
        }

        public DateTime Created
        {
            get;
            set;
        }

        public DirectoryInfo Directory
        {
            get;
            set;
        }

        public int Day;
        public int Number;
        private static Regex TemplateRe = new Regex("y(\\d+)\\\\m(\\d+)\\\\d([\\d+]+)_post([\\d+]+$)", RegexOptions.Compiled);

        public string Name { get; internal set; }

        public static Post TryParse(Context context, Blog blog, string contentDir)
        {
            var m = TemplateRe.Match(contentDir);
            if(m.Success)
            {
                return new Post(context, blog)
                {
                    Name = contentDir,
                    Created = new DateTime(
                        Convert.ToInt32(m.Groups[1].Value), 
                        Convert.ToInt32(m.Groups[2].Value), 
                        Convert.ToInt32(m.Groups[3].Value)
                    ),
                    Number = Convert.ToInt32(m.Groups[4].Value),
                    Directory = new DirectoryInfo(contentDir)
                };
            }

            return null;
        }    

        private bool IsTemplateContentFile(FileInfo fileInfo)
        {
            return fileInfo.FullName.EndsWith(".md");
        }

        private FileInfo _File = null;
        public FileInfo File
        {
            get
            {
                if(_File == null)
                {
                    foreach(var fileInfo in Directory.EnumerateFiles("*.md"))
                    {
                        if(IsTemplateContentFile(fileInfo))
                        {
                            _File = fileInfo;
                        }
                    }
                }

                return _File;
            }
        }

        public string MdContent
        {
            get
            {
                return System.IO.File.ReadAllText(File.FullName);
            }
        }

        public string Url 
        { 
            get
            {
                return Directory.Parent.Parent.Name + "\\" + Directory.Parent.Name + "\\" + Directory.Name + "\\" + File.Name + ".html";
            }
        }
        
        public override string GetVariableValue(Variable variable) => variable.Name switch
        {
            Global.VAR_NAME_POST_TITLE => Title,
            Global.VAR_NAME_POST_TITLE_TILE => TitleTile,
            Global.VAR_NAME_POST_CREATED => ToLocaleDate(Created),
            Global.VAR_NAME_POST_URL => Url,
            Global.VAR_NAME_POST_CONTENT => MdConverter.Convert(MdContent),
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