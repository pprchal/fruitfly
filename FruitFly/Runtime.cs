// Pavel Prchal, 2023

using System;
using System.Collections.Generic;

namespace fruitfly
{
    public static class Runtime
    {
        public static readonly DateTime StartTime = DateTime.Now;

        public static IConsole Console
        {
            get;
            private set;
        }

        public static IConfiguration Configuration
        {
            get;
            private set;
        }

        public static IVariableSource VariableSource
        {
            get;
            private set;
        }

        public static IStorage Storage
        {
            get;
            private set;
        }

        public static IConverter Converter
        {
            get;
            private set;
        }

        internal static void Start()
        {
            Console = new Console();

            var yamlConfig = new YamlConfiguration();
            Configuration = yamlConfig;
            VariableSource = yamlConfig;
            Storage = new plugins.FileStorage();
            Converter = new plugins.MarkdigHtmlConverter();
        }
    }
}
