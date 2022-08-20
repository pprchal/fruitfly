// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly.objects
{
    public class Template : AbstractTemplate
    {
        private readonly string _TemplateName;

        public Template(AbstractTemplate parent, string templateName, IVariableSource configSource, IStorage storage) : base(configSource, parent, storage)
        {
            _TemplateName = templateName;
        }

        public override string TemplateName => _TemplateName;
    }
}