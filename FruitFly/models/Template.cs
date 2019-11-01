// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly.objects
{
    public class Template : AbstractContentObject
    {
        public Template(AbstractContentObject parent) : base(parent)
        {
        }

        public override string GetVariableValue(Variable variable) 
        {
            return Parent.GetVariableValue(variable);
        }
    }
}