// Pavel Prchal, 2023

using System;
using System.Collections.Generic;

namespace fruitfly
{
    public static class Runtime
    {
        public static readonly DateTime StartTime = DateTime.Now;

        public static void WriteLine(string msg) => System.Console.WriteLine($"{msg}");

        public static YamlConfiguration Configuration
        {
            get;
            private set;
        }

        public static IVariableSource VariableSource
        {
            get;
            private set;
        }

        public static IConverter Converter
        {
            get;
            private set;
        }

        public static void Start()
        {
            var yamlConfig = new YamlConfiguration();
            Configuration = yamlConfig;
            VariableSource = yamlConfig;
            Converter = new plugins.MarkdigHtmlConverter();
        }
    }
}
