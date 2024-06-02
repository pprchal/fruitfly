// Pavel Prchal, 2019, 2023

using System.Collections.Generic;
using System.Threading.Tasks;

namespace fruitfly
{
    public abstract class AbstractTemplate : IVariableSource
    {
        public AbstractTemplate(IVariableSource parent)
        {
            Parent = parent;
        }

        public AbstractTemplate()
        {
            Parent = null;
        }

        public virtual async Task<string> Render(string morph = null) =>
            await new VariableBinder()
                .Bind(
                    content: await FileStorage.LoadTemplate(TemplateName),
                    variableSource: this
                );
        
        public abstract string TemplateName { get; }
        
        readonly IList<AbstractTemplate> ChildParts = [];

        protected IVariableSource Parent
        {
            get;
            set;
        }

        public virtual async Task<string> GetVariableValue(Variable variable)
        {
            switch (variable.Scope)
            {
                case Constants.Scope.CONFIG:
                    return await Runtime.VariableSource.GetVariableValue(variable);

                case Constants.Scope.TEMPLATE:
                    var nestedTemplate = new Template(
                        parent: this,
                        templateName: variable.Name
                    );
                    ChildParts.Add(nestedTemplate);
                    return await nestedTemplate.Render();
            }

            if (Parent != null)
            {
                return await Parent.GetVariableValue(variable);
            }

            return $"Unknown variable: [{variable}] for template-{GetType().Name}: [{TemplateName}]";
        }

        public virtual string[] BuildStoragePath() => [];
    }
}