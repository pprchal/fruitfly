// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Globalization;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Post : AbstractTemplate
    {
        public Post(AbstractTemplate parent) : base(parent)
        {
        }

        public override string TemplateName =>
            Global.TEMPLATE_POST;
        
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

        public string TitleTile =>
            Title;

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

        public string MdContent =>
            Context
            .GetLogic<Storage>()
            .LoadContentByStorageId(StorageId);

        public override string Render(RenderedFormats renderedFormats, string morph = null)
        {
            if(morph == Global.MORPH_TILE)
            {
                return Context.GetLogic<VariableBinder>().Bind(
                    Context.GetLogic<Storage>().LoadTemplate("postTile.html"),
                    this
                ).ToString();
            }
            return base.Render(renderedFormats, morph);
        }

        public override List<string> BuildFolderStack() =>
            new List<string>()
            {
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            };

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
            { Scope: "post", Name: Global.VAR_NAME_POST_CONTENT }  => Context.Current.MdConverter.Convert(MdContent),
            _ => Parent.GetVariableValue(variable)
        };

        private CultureInfo Culture =>
            new CultureInfo(Context.Current.Config.language.Replace("_", "-"));

        private string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);
    }
}