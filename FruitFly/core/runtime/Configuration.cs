// Pavel Prchal, 2019

using System.Dynamic;
using System.IO;
using fruitfly.core;
using YamlDotNet.Serialization;

namespace fruitfly.objects
{
    public class Configuration : IVariableSource
    {
        public Configuration()
        {
            YamlConfig = LoadYamlConfig();
        }
        
        static dynamic LoadYamlConfig() =>
            new DeserializerBuilder()
                .Build()
                .Deserialize<ExpandoObject>(
                    new StringReader(
                        File.ReadAllText(Constants.Config.YML)
                    )
                );

        dynamic YamlConfig;
                
        public string templateDir => YamlConfig.templateDir;
        public string workDir => YamlConfig.workDir;
        public string language => YamlConfig.language;
        public string home => YamlConfig.home;
        public string title => YamlConfig.title;
        public string template => YamlConfig.template;
        public string fullVersion => "6.0";

        string IVariableSource.GetVariableValue(Variable variable) =>
            GetType()
            .InvokeMember(
                name: variable.Name,
                invokeAttr: System.Reflection.BindingFlags.GetProperty, 
                binder: null, 
                target: this, 
                args: null
            ) as string;
    }
}
