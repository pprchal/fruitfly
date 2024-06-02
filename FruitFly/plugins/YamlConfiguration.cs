// Pavel Prchal, 2019

using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace fruitfly
{
    public class YamlConfiguration : IVariableSource
    {
        public YamlConfiguration()
        {
            YamlConfig = LoadYamlConfig();
        }
        
        static ExpandoObject LoadYamlConfig() =>
            new DeserializerBuilder()
                .Build()
                .Deserialize<ExpandoObject>(
                    new StringReader(
                        File.ReadAllText(Constants.Config.YML)
                    )
                );

        readonly dynamic YamlConfig;
                
        public string templateDir
        {
            get
            {
                try
                {
                    return YamlConfig.templateDir;
                }
                catch
                {
                }
                return Path.Combine(YamlConfig.workDir, Constants.Templates.FOLDER);
            }
        }

        public string workDir => YamlConfig.workDir;
        public string language => YamlConfig.language;
        public string home => YamlConfig.home;
        public string title => YamlConfig.title;
        public string template => YamlConfig.template;
        public string fullVersion => "6.0";

        Task<string> IVariableSource.GetVariableValue(Variable variable)
        {
            var variableValue = ConfigCache.GetOrAdd(
                key: variable.Name,
                valueFactory: (_) => ResolveVariable(variable)
            );
            return Task.FromResult(variableValue);
        }

        readonly ConcurrentDictionary<string, string> ConfigCache = new();

        string ResolveVariable(Variable variable)
        {
            try
            {
                // fruitfly.objects.IConfiguration.get_language
                var mi = GetType()
                    // .GetInterfaceMap(typeof(IConfiguration))
                    .GetMethods()
                    .FirstOrDefault(m => m.Name.EndsWith("_" + variable.Name));
                return (string)mi.Invoke(this, null);
            }
            catch(Exception)
            {
                return $"ERROR::{variable}";
            }
        }
    }
}
