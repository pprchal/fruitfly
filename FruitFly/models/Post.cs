// Pavel Prchal, 2019, 2023

using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace fruitfly
{
    public class Post : AbstractTemplate
    {
        readonly IConfiguration Configuration = Runtime.Get<IConfiguration>();
        readonly IConverter Converter = Runtime.Get<IConverter>();

        public Post(
            string name,
            string title,
            string storageId,
            DateTime created,
            int number
        ) : base()
        {
            Name = name;
            Title = title;
            StorageId = storageId;
            Created = created;
            Number = number;
        }

        public override string TemplateName => Constants.Templates.POST;
        
        public readonly string Name;

        public readonly string Title;

        public string TitleTile => Title;

        public readonly DateTime Created;

        public readonly string StorageId;

        public readonly int Number;

        public async Task<string> GetContent() =>
            await Storage.LoadContentByStorageId(StorageId);

        public override async Task<string> Render(string morph = null) =>
            morph == Constants.MORPH_TILE
            ?
                await new VariableBinder().Bind(
                    content: await Storage.LoadTemplate(Constants.Templates.POST_TILE),
                    variableSource: this
                )
            :
            await base.Render(morph);
        
        public override string[] BuildStoragePath() =>
            new string[]
            {
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            };

        public string Url  => string.Join(
            separator: "\\",
            values: BuildStoragePath().Concat(new string[] { Name + ".html"})
        );
        
        public override async Task<string> GetVariableValue(Variable variable) => variable switch
        {
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_TITLE }  => Title,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_TITLE_TILE }  => TitleTile,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_CREATED }  => ToLocaleDate(Created),
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_URL } => Url,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_CONTENT }  => Converter.Convert(await GetContent()),
            _ => await Parent.GetVariableValue(variable)
        };

        CultureInfo Culture =>
            new(Configuration.language.Replace("_", "-"));

        string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);

        internal void SetParent(Blog blog)
        {
            Parent = blog;
        }
    }
}