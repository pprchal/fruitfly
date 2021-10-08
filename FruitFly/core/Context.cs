// Pavel Prchal, 2019, 2020

using System;
using fruitfly.objects;

namespace fruitfly.core
{
    public static class Context
    {
        public static Action<string> ConsoleWrite
        {
            get;
            set;
        } = (msg) => Console.WriteLine(msg);

        public static IMdConverter MdConverter
        {
            get;
            set;
        } = new MarkdigHtmlConverter();

        public static IStorage Storage
        {
            get;
            set;
        } = new Storage();
        

        public static Configuration Config
        {
            get;
            set;
        }

        public static DateTime StartTime
        {
            get;
            set;
        } = DateTime.Now;
    }
}
