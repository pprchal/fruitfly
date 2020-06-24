// Pavel Prchal, 2019

using System;
using fruitfly.core;

namespace fruitfly.objects
{
    [Serializable]
    public class Configuration : IVariableSource
    {
        public string templateDir
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

        public string fullVersion =>
            "5.0";

        string IVariableSource.GetVariableValue(Variable variable) =>
            GetType()
            .InvokeMember(
                name: variable.Name,
                invokeAttr: System.Reflection.BindingFlags.GetProperty, 
                binder: null, 
                target: this, 
                args: null
            ) as string;
    }
}
