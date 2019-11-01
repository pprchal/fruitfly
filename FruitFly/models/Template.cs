// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Template : AbstractContentObject
    {
        public Template(Context context, AbstractContentObject parent) : base(context, parent)
        {
        }

        public override List<string> BuildFolderStack()
        {
            throw new System.NotImplementedException();
        }

        public override string GetVariableValue(Variable variable) 
        {
            return Parent.GetVariableValue(variable);
        }
    }
}