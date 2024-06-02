// Pavel Prchal, 2019, 2023

using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace fruitfly
{
    public class Post(
        string name,
        string title,
        string storageId,
        DateTime created,
        int number
        ) : AbstractTemplate()
    {
        public override string TemplateName => Constants.Templates.POST;
        
        public readonly string Name = name;

        public readonly string Title = title;

        public string TitleTile => Title;

        public readonly DateTime Created = created;

        public readonly string StorageId = storageId;

        public readonly int Number = number;

        public async Task<string> GetContent() =>
            await Runtime.Storage.LoadContentByStorageId(StorageId);

        public override async Task<string> Render(string morph = null) =>
            morph == Constants.MORPH_TILE
            ?
                await new VariableBinder().Bind(
                    content: await Runtime.Storage.LoadTemplate(Constants.Templates.POST_TILE),
                    variableSource: this
                )
            :
            await base.Render(morph);
        
        public override string[] BuildStoragePath() =>
            [
                $"y{Created.Year}",
                $"m{Created.Month}",
                $"d{Created.Day}_post{Number}"
            ];

        public string Url  => string.Join(
            separator: "\\",
            values: BuildStoragePath().Concat([Name + ".html"])
        );
        
        public override async Task<string> GetVariableValue(Variable variable) => variable switch
        {
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_TITLE }  => Title,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_TITLE_TILE }  => TitleTile,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_CREATED }  => ToLocaleDate(Created),
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_URL } => Url,
            { Scope: Constants.Scope.POST, Name: Constants.Variables.POST_CONTENT }  => Runtime.Converter.Convert(await GetContent()),
            _ => await Parent.GetVariableValue(variable)
        };

        CultureInfo Culture =>
            new(Runtime.Configuration.language.Replace("_", "-"));

        string ToLocaleDate(DateTime date) =>
            date.ToString("d", Culture);

        internal void SetParent(IVariableSource parent)
        {
            Parent = parent;
        }
    }
}