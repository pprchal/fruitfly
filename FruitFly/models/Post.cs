// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Globalization;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Post : AbstractTemplate
    {
        public Post(AbstractTemplate parent, IStorage storage) : base(parent, storage)
        {
        }

        public override string TemplateName => Constants.Templates.POST;
        
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

        public string TitleTile => Title;

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

        public string Content =>
            Storage.LoadContentByStorageId(StorageId);

        IConverter Converter;
        public override string Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return morph == Constants.MORPH_TILE
            ?
                new VariableBinder().Bind(
                    content: Storage.LoadTemplate(Constants.Templates.POST_TILE),
                    variableSource: this
                ).ToString()
            :
            base.Render(converter, morph);
        }
        
        public override IList<string> BuildStoragePath() =>
            new List<string>
            {
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            };

        public string Url 
        { 
            get
            {
                var storagePath = BuildStoragePath();
                storagePath.Add(Name + ".html");
                return string.Join("\\", storagePath);
            }
        }
        
        public override string GetVariableValue(Variable variable) => variable switch
        {
            { Scope: "post", Name: Constants.Variables.POST_TITLE }  => Title,
            { Scope: "post", Name: Constants.Variables.POST_TITLE_TILE }  => TitleTile,
            { Scope: "post", Name: Constants.Variables.POST_CREATED }  => ToLocaleDate(Created),
            { Scope: "post", Name: Constants.Variables.POST_URL } => Url,
            { Scope: "post", Name: Constants.Variables.POST_CONTENT }  => Converter.Convert(Content),
            _ => Parent.GetVariableValue(variable)
        };

        CultureInfo Culture =>
            new CultureInfo(Context.Config.language.Replace("_", "-"));

        string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);
    }
}