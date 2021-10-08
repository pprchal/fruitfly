// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using fruitfly.core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using YamlDotNet.Serialization;

namespace fruitfly.objects
{
    public class Configuration : IVariableSource
    {
        Configuration()
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
                
        public string TemplateDir => YamlConfig.templateDir;
        public string WorkDir => YamlConfig.workDir;
        public string Language => YamlConfig.language;
        public string Home => YamlConfig.home;
        public string Title => YamlConfig.title;
        public string Template => YamlConfig.template;

        public string FullVersion => "5.0";

        string IVariableSource.GetVariableValue(Variable variable) =>
            GetType()
            .InvokeMember(
                name: variable.Name,
                invokeAttr: System.Reflection.BindingFlags.GetProperty, 
                binder: null, 
                target: this, 
                args: null
            ) as string;

        // string GetVariableValue(string variableName, object target) =>
        //     GetType()
        //     .InvokeMember(
        //         name: variableName,
        //         invokeAttr: System.Reflection.BindingFlags.GetProperty, 
        //         binder: null, 
        //         target: target, 
        //         args: null
        //     ) as string;

    }
}
