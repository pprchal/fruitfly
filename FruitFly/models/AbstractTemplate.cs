// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractTemplate : IVariableSource
    {
        protected Context Context { get; }

        public virtual string Render(RenderedFormats renderedFormats, string morph = null)
        {
            return Context.GetLogic<VariableBinder>().Bind(
                Context.GetLogic<Storage>().LoadTemplate(TemplateName),
                this
            ).ToString();
        }
        
        public abstract string TemplateName { get; }
        
        public List<AbstractTemplate> ChildParts
        {
            get;
        } = new List<AbstractTemplate>();

        public AbstractTemplate(Context context, AbstractTemplate parent)
        {
            Context = context;
            Parent = parent;
        }

        protected virtual string RenderNestedTemplateByVariable(Variable variable)
        {
            var nestedTemplate = new Template(Context, this, variable.Name);
            ChildParts.Add(nestedTemplate);
            return nestedTemplate.Render(RenderedFormats.Html);
        }

        public AbstractTemplate Parent
        {
            get;
        }

        public virtual string GetVariableValue(Variable variable)
        {
            if(Parent != null)
            {
                return Parent.GetVariableValue(variable);
            }

            throw new System.Exception($"Unknown variable: [{variable.ReplaceBlock}] for template: [{TemplateName}]");
        }

        public virtual List<string> BuildFolderStack()
        {
            return new List<string>();
        }  
    }
}