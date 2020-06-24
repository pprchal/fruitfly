// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractTemplate : IVariableSource
    {
        public virtual string Render(RenderedFormats renderedFormats, string morph = null) =>
            Context.GetLogic<VariableBinder>()
            .Bind(
                Context.GetLogic<Storage>().LoadTemplate(TemplateName),
                this
            ).ToString();
        
        public abstract string TemplateName { get; }
        
        public List<AbstractTemplate> ChildParts
        {
            get;
        } = new List<AbstractTemplate>();

        public AbstractTemplate(AbstractTemplate parent)
        {
            Parent = parent;
        }

        protected virtual string RenderNestedTemplateByVariable(Variable variable)
        {
            var nestedTemplate = new Template(this, variable.Name);
            ChildParts.Add(nestedTemplate);
            return nestedTemplate.Render(RenderedFormats.Html);
        }

        public AbstractTemplate Parent
        {
            get;
        }

        private IVariableSource ConfigVariableSource =>
            Context.Current.Config as IVariableSource;

        public virtual string GetVariableValue(Variable variable)
        {
            if(variable.Scope == Global.SCOPE_NAME_CONFIG)
            {
                return ConfigVariableSource.GetVariableValue(variable);
            }
            else if(variable.Scope == Global.SCOPE_NAME_TEMPLATE)
            {
                return RenderNestedTemplateByVariable(variable);
            }

            if(Parent != null)
            {
                return Parent.GetVariableValue(variable);
            }

            throw new System.Exception($"Unknown variable: [{variable.ReplaceBlock}] for template: [{TemplateName}]");
        }

        public virtual List<string> BuildFolderStack() =>
            new List<string>();
    }
}