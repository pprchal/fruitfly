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
        } = "en_US";

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
