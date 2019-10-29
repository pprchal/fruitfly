using System;

namespace fruitfly
{
    [Serializable]
    public class Configuration : IVariableProvider
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

        string IVariableProvider.GetVariableValue(string name)
        {
            var propName = name.Split(":")[1];
            return (string) GetType().InvokeMember(propName, System.Reflection.BindingFlags.GetProperty, null, this, null);
        }        
    }
}
