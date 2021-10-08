// Pavel Prchal, 2019, 2020

using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractTemplate : IVariableSource
    {
        
        public abstract string TemplateName { get; }

        public AbstractTemplate(AbstractTemplate parent)
        {
            Parent = parent;
        }

        public virtual string Render(RenderedFormats renderedFormats, string morph = null) =>
            TemplateProcessor
                .Process(
                    content: Context.Storage.LoadTemplate(TemplateName),
                    variableSource: this,
                    diag: TemplateName
                );


        protected virtual string RenderNestedTemplateByVariable(Variable variable) =>
            new Template(
                parent: this,
                templateName: variable.Name
            )
            .Render(RenderedFormats.Html);

        public AbstractTemplate Parent;

        private IVariableSource ConfigVariableSource =>
            Context.Config;

        public virtual string GetVariableValue(Variable variable)
        {
            if(variable.Scope == Constants.Config.Scope)
            {
                return ConfigVariableSource.GetVariableValue(variable);
            }
            else if(variable.Scope == Constants.Templates.Scope)
            {
                return RenderNestedTemplateByVariable(variable);
            }

            if(Parent != null)
            {
                return (Parent as IVariableSource).GetVariableValue(variable);
            }

            throw new System.Exception($"Unknown variable: [{variable.ReplaceBlock}] for template: [{TemplateName}]");
        }
    }
}