// Pavel Prchal, 2023

using System;
using System.Collections.Generic;

namespace fruitfly
{
    public sealed class Runtime
    {
        static public readonly DateTime StartTime = DateTime.Now;

        private static readonly Dictionary<Type, object> _Services = new();

        internal static void CreateAndRegister()
        {
            Add<IConsole>(new Console());
            var configuration = new YamlConfiguration();
            Add<IConfiguration>(configuration);
            Add<IVariableSource>(configuration);
            Add<IStorage>(new plugins.FileStorage());
            Add<IConverter>(new plugins.MarkdigHtmlConverter());
        }

        public static void Add<T>(object service) where T : class =>
            _Services.Add(typeof(T), service);

        public static T Get<T>() where T : class =>
            _Services[typeof(T)] as T;
    }
}
