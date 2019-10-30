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
        private Context()
        {

        }

        public IConsole Console
        {
            get;
        } = new Console();

        public Configuration Config
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
        } = DateTime.Now;

        private Dictionary<Type, BaseLogic> Singletons = new Dictionary<Type, BaseLogic>();
        public T GetLogic<T>() where T : BaseLogic, new()
        {
            var type = typeof(T);

            if(!Singletons.ContainsKey(type))
            {
                Singletons.Add(type, new T() { Context = this });
            }

            return Singletons[type] as T;
        }

        public static Context CreateContext()
        {
            return new Context()
            {
                Config = LoadYmlConfig()
            };
        }

        private static Configuration LoadYmlConfig()
        {
            return new DeserializerBuilder()
                .Build()
                .Deserialize<Configuration>(new StringReader(File.ReadAllText(Global.CONFIG_YML)));
        }
    }
}
