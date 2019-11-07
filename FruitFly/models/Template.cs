// Pavel Prchal, 2019

using fruitfly.core;

namespace fruitfly.objects
{
    public class Template : AbstractTemplate
    {
        private readonly string _TemplateName;

        public Template(Context context, AbstractTemplate parent, string templateName) : base(context, parent)
        {
            _TemplateName = templateName;
        }

        public override string TemplateName => _TemplateName;
    }
}