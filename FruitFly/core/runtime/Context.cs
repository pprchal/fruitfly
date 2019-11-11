// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.IO;
using fruitfly.objects;
using YamlDotNet.Serialization;

namespace fruitfly.core
{
    public class Context : IContext
    {
        public Context()
        {
        }

        public IConsole Console
        {
            get;
        } = new Console();

        Configuration _Configuration = null;
        public Configuration Config
        {
            get
            {
                if(_Configuration == null)
                {
                    _Configuration = (this as IContext).CreateConfig();
                }

                return _Configuration;
            }
        }

        public DateTime StartTime
        {
            get;
        } = DateTime.Now;
        
        private Dictionary<Type, AbstractLogic> Singletons = new Dictionary<Type, AbstractLogic>();
        public virtual T GetLogic<T>() where T : AbstractLogic, new()
        {
            var type = typeof(T);

            if(!Singletons.ContainsKey(type))
            {
                Singletons.Add(type, new T() { Context = this });
            }

            return Singletons[type] as T;
        }

        protected virtual Configuration CreateYamlConfig(TextReader yamlReader)
        {
            return new DeserializerBuilder()
                .Build()
                .Deserialize<Configuration>(yamlReader);
        }

        public virtual Configuration CreateConfig()
        {
            return CreateYamlConfig(new StringReader(File.ReadAllText(Global.CONFIG_YML)));
        }
    }
}
