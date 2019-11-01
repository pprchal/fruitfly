// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Globalization;
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

        public int Number
        {
            get;
            set;
        }

        public string MdContent
        {
            get
            {
                return Context.GetLogic<Storage>().LoadByStorageId(StorageId);
            }
        }

        public override List<string> BuildFolderStack()
        {
            return new List<string>()
            {
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            };
        }        

        public string Url 
        { 
            get
            {
                var urlFolderStack = BuildFolderStack();
                urlFolderStack.Add(Name + ".html");
                return string.Join("\\", urlFolderStack);
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

        private CultureInfo Culture
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

        private IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();
    }
}