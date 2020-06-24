// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.IO;
using fruitfly.objects;
using YamlDotNet.Serialization;

namespace fruitfly.core
{
    public class Context
    {
        public static Context Current = new Context();

        public IConsole Console
        {
            get;
        } = new Console();

        public IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();

        Configuration _Configuration = null;
        public Configuration Config
        {
            get
            {
                if(_Configuration == null)
                {
                    _Configuration = CreateConfig();
                }

                return _Configuration;
            }
        }

        public DateTime StartTime
        {
            get;
        } = DateTime.Now;
        
        private Dictionary<Type, AbstractLogic> Singletons = new Dictionary<Type, AbstractLogic>();

        private T GetLogicInternal<T>() where T : AbstractLogic, new()
        {
            var type = typeof(T);

            if(!Singletons.ContainsKey(type))
            {
                Singletons.Add(type, new T());
            }

            return Singletons[type] as T;
        }

        public static T GetLogic<T>() where T : AbstractLogic, new() =>
            Current.GetLogicInternal<T>();

        protected Configuration CreateYamlConfig(TextReader yamlReader) =>
            new DeserializerBuilder()
                .Build()
                .Deserialize<Configuration>(yamlReader);

        public virtual Configuration CreateConfig() =>
            CreateYamlConfig(new StringReader(File.ReadAllText(Global.CONFIG_YML)));
    }
}
