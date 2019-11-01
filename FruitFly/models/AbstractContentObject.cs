// Pavel Prchal, 2019

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

        public AbstractContentObject Parent
        {
            get;
        }

        public virtual string GetVariableValue(Variable variable)
        {
            return "ggg----gggg";
            // if(variable.Scope == Global.TEMPLATE)
            // {
            //     Context.GetLogic<VariableBinder>().Bind(
            //         Context.GetLogic<Storage>().LoadTemplate(variable.Name),
            //         Parent);

            // }
        }
    }
}