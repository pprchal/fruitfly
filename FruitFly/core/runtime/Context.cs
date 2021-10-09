// Pavel Prchal, 2019

using System;
using fruitfly.objects;

namespace fruitfly.core
{
    public class Context
    {
        static Configuration _Configuration = null;
        public static Configuration Config
        {
            get
            {
                if(_Configuration == null)
                {
                    _Configuration = new Configuration();
                }

                return _Configuration;
            }
        }

        public static DateTime StartTime
        {
            get;
        } = DateTime.Now;
    }
}
