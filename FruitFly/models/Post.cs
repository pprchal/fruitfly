// Pavel Prchal, 2019, 2020

using System;
using System.Linq;
using System.Globalization;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Post : AbstractTemplate, IStorageContent
    {
        public Post(AbstractTemplate parent) : base(parent)
        {
        }

        public override string TemplateName =>
            Constants.Templates.Post;
        
        public string Name;

        public string Title;

        public string TitleTile =>
            Title;

        public DateTime Created;

        public string StoragePath;

        public int Number;


        public override string Render(RenderedFormats format, string morph = null)
        {
            if(morph == Constants.Blog.Tile)
            {
                return TemplateProcessor.Process(
                    content: Context.Storage.LoadTemplate(Constants.Templates.Tile),
                    variableSource: this,
                    diag: Constants.Templates.Tile
                );
            }
            return base.Render(format, morph);
        }

        string[] IStorageContent.BuildFolderStack() =>
            new string[]
            {
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            };

        private string Url 
        { 
            get
            {
                return string
                    .Join("\\", (this as IStorageContent).BuildFolderStack().Concat(new string[] { Name + ".html" }));
            }
        }
        
        public override string GetVariableValue(Variable variable) => variable switch
        {
            { Scope: Constants.Post.Scope, Name: Constants.Post.Title }  => Title,
            { Scope: Constants.Post.Scope, Name: Constants.Post.TitleTile }  => TitleTile,
            { Scope: Constants.Post.Scope, Name: Constants.Post.Created }  => ToLocaleDate(Created),
            { Scope: Constants.Post.Scope, Name: Constants.Post.Url } => Url,
            { Scope: Constants.Post.Scope, Name: Constants.Post.Content }  => Context.MdConverter.Convert(GetMdContent()),
            _ => (Parent as IVariableSource).GetVariableValue(variable)
        };


        private string GetMdContent() =>
            Context
                .Storage
                .LoadContent(StoragePath);

        private CultureInfo Culture =>
            new CultureInfo(Context.Config.language.Replace("_", "-"));

        private string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);
    }
}