// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractContentObject : IVariableSource
    {
        private AbstractContentObject()
        {

        }

        public AbstractContentObject(AbstractContentObject parent)
        {
            Parent = parent;
        }

        public AbstractContentObject Parent
        {
            get;
        }


        public abstract string GetVariableValue(Variable variable);
    }
}