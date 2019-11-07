// Pavel Prchal, 2019

using System;
using fruitfly.core;

namespace fruitfly.objects
{
    [Serializable]
    public class Configuration : IVariableSource
    {
        public string rootDir
        {
            get;
            set;
        } = "";

        public string workDir
        {
            get;
            set;
        }
        
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
            get => "5.0";
        }

        string IVariableSource.GetVariableValue(Variable variable)
        {
            return GetType().InvokeMember(variable.Name, System.Reflection.BindingFlags.GetProperty, null, this, null) as string;
        }
    }
}
