// Pavel Prchal, 2019

using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using fruitfly.core;
using YamlDotNet.Serialization;

namespace fruitfly.objects
{
    public class Configuration : IConfiguration, IVariableSource
    {
        public Configuration()
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

        dynamic YamlConfig;
                
        string IConfiguration.templateDir
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

        string IConfiguration.workDir => YamlConfig.workDir;
        string IConfiguration.language => YamlConfig.language;
        string IConfiguration.home => YamlConfig.home;
        string IConfiguration.title => YamlConfig.title;
        string IConfiguration.template => YamlConfig.template;
        string IConfiguration.fullVersion => "6.0";

        Task<string> IVariableSource.GetVariableValue(Variable variable) =>
            Task.FromResult(
                GetType()
                    .InvokeMember(
                        name: variable.Name,
                        invokeAttr: System.Reflection.BindingFlags.GetProperty, 
                        binder: null, 
                        target: this, 
                        args: null
                    ) as string
            );
    }
}
