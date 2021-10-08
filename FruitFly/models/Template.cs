// Pavel Prchal, 2019

namespace fruitfly.objects
{
    public class Template : AbstractTemplate
    {
        private readonly string _TemplateName;

        public Template(AbstractTemplate parent, string templateName) : base(parent)
        {
            _TemplateName = templateName;
        }

        public override string TemplateName => _TemplateName;
    }
}