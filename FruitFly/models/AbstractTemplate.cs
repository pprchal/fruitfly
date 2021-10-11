// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Threading.Tasks;
using fruitfly.core;

namespace fruitfly.objects
{
    public abstract class AbstractTemplate : IVariableSource
    {
        IConverter Converter;  // injected by method, not constructor!
        protected readonly IStorage Storage;

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

        public virtual async Task<string> Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return await new VariableBinder()
                .Bind(
                    content: await Storage.LoadTemplate(TemplateName),
                    variableSource: this
                );
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

        static IVariableSource ConfigVariableSource => Context.Config;

        public virtual async Task<string> GetVariableValue(Variable variable)
        {
            if(variable.Scope == Constants.Scope.CONFIG)
            {
                return await ConfigVariableSource.GetVariableValue(variable);
            }
            else if(variable.Scope == Constants.Scope.TEMPLATE)
            {
                var nestedTemplate = new Template(
                    parent: this, 
                    templateName: variable.Name,
                    storage: Storage
                );
                ChildParts.Add(nestedTemplate);
                return await nestedTemplate.Render(Converter);  
            }

            if(Parent != null)
            {
                return await Parent.GetVariableValue(variable);
            }

            throw new System.Exception($"Unknown variable: [{variable.ReplaceBlock}] for template: [{TemplateName}]");
        }

        public virtual IList<string> BuildStoragePath() => new List<string>();
    }
}