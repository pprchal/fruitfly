using System;

namespace fruitfly
{
    [Serializable]
    public class Configuration
    {
        public string language
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
    }
}
