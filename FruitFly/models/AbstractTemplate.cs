// Pavel Prchal, 2019, 2023

using System.Collections.Generic;
using System.Threading.Tasks;

namespace fruitfly
{
    public abstract class AbstractTemplate : IVariableSource
    {
        protected IStorage Storage => Runtime.Get<IStorage>();
        IVariableSource ConfigSource => Runtime.Get<IVariableSource>();

        public AbstractTemplate(AbstractTemplate parent)
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
                    content: await Storage.LoadTemplate(TemplateName),
                    variableSource: this
                );
        
        public abstract string TemplateName { get; }
        
        readonly IList<AbstractTemplate> ChildParts = new List<AbstractTemplate>();

        protected AbstractTemplate Parent
        {
            get;
            set;
        }

        public virtual async Task<string> GetVariableValue(Variable variable)
        {
            switch (variable.Scope)
            {
                case Constants.Scope.CONFIG:
                    return await ConfigSource.GetVariableValue(variable);

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

        public virtual string[] BuildStoragePath() => new string[] {};
    }
}