// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractContentObject : IVariableSource
    {
        public AbstractContentObject Parent
        {
            get;
            set;
        }

        public List<AbstractContentObject> Objects
        {
            get;
        } = new List<AbstractContentObject>();

        public string Html
        {
            get;
            set;
        }

        public abstract string GetVariableValue(string name);
    }
}