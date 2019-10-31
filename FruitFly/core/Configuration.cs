// Pavel Prchal, 2019

using System;
using fruitfly.core;

namespace fruitfly.objects
{
    [Serializable]
    public class Configuration : IVariableSource
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

        string IVariableSource.GetVariableValue(string name)
        {
            return this.GetType().InvokeMember(name, System.Reflection.BindingFlags.GetProperty, null, this, null) as string;
        }
    }
}
