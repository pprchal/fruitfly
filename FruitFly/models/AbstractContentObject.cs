// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractContentObject : IVariableSource
    {
        protected Context Context { get; }

        public AbstractContentObject(Context context, AbstractContentObject parent)
        {
            Context = context;
            Parent = parent;
        }

        protected virtual string HandleTemplate(Variable variable)
        {
            return Context.GetLogic<VariableBinder>().Bind(
                Context.GetLogic<Storage>().LoadTemplate(variable.Name),
                this
            ).ToString();
        }

        public AbstractContentObject Parent
        {
            get;
        }

        public abstract string GetVariableValue(Variable variable);

        public abstract List<string> BuildFolderStack();
    }
}