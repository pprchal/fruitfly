// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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

        public async Task<string> GetContent() =>
            await Storage.LoadContentByStorageId(StorageId);


        IConverter Converter;
        public override async Task<string> Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return morph == Constants.MORPH_TILE
            ?
                await new VariableBinder().Bind(
                    content: await Storage.LoadTemplate(Constants.Templates.POST_TILE),
                    variableSource: this
                )
            :
            await base.Render(converter, morph);
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
        
        public override async Task<string> GetVariableValue(Variable variable) => variable switch
        {
            { Scope: "post", Name: Constants.Variables.POST_TITLE }  => Title,
            { Scope: "post", Name: Constants.Variables.POST_TITLE_TILE }  => TitleTile,
            { Scope: "post", Name: Constants.Variables.POST_CREATED }  => ToLocaleDate(Created),
            { Scope: "post", Name: Constants.Variables.POST_URL } => Url,
            { Scope: "post", Name: Constants.Variables.POST_CONTENT }  => Converter.Convert(await GetContent()),
            _ => await Parent.GetVariableValue(variable)
        };

        CultureInfo Culture =>
            new(Context.Config.language.Replace("_", "-"));

        string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);
    }
}