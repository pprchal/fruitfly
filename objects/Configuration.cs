// Pavel Prchal, 2019

using System;

namespace fruitfly.objects
{
    [Serializable]
    public class Configuration
    {
        public string language
        {
            get;
            set;
        }

        public string home
        {
            get;
            set;
        }
        
        public string title
        {
            get;
            set;
        } = "blog";

        public string template
        {
            get;
            set;
        } = "default";

        public string fullVersion
        {
            get => "1.0 preview";
        }

    }
}
