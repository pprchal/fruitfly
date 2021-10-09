// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractTemplate : IVariableSource
    {
        protected IStorage Storage;

        public AbstractTemplate(AbstractTemplate parent, IStorage storage)
        {
            Parent = parent;
            Storage = storage;
        }

        public AbstractTemplate(IStorage storage)
        {
            Parent = null;
            Storage = storage;
        }

        IConverter Converter;
        public virtual string Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return new VariableBinder()
                .Bind(
                    content: Storage.LoadTemplate(TemplateName),
                    variableSource: this
                ).ToString();
        }
        
        public abstract string TemplateName { get; }
        
        public IList<AbstractTemplate> ChildParts
        {
            get;
        } = new List<AbstractTemplate>();

        public AbstractTemplate Parent
        {
            get;
        }

        IVariableSource ConfigVariableSource =>
            Context.Config as IVariableSource;

        public virtual string GetVariableValue(Variable variable)
        {
            if(variable.Scope == Constants.Scope.CONFIG)
            {
                return ConfigVariableSource.GetVariableValue(variable);
            }
            else if(variable.Scope == Constants.Scope.TEMPLATE)
            {
                var nestedTemplate = new Template(
                    parent: this, 
                    templateName: variable.Name,
                    storage: Storage
                );
                ChildParts.Add(nestedTemplate);
                return nestedTemplate.Render(Converter);  
            }

            if(Parent != null)
            {
                return Parent.GetVariableValue(variable);
            }

            throw new System.Exception($"Unknown variable: [{variable.ReplaceBlock}] for template: [{TemplateName}]");
        }

        public virtual IList<string> BuildStoragePath() =>
            new List<string>();
    }
}