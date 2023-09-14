// Pavel Prchal, 2019

namespace fruitfly
{
    public class Template : AbstractTemplate
    {
        public Template(AbstractTemplate parent, string templateName) : base(parent)
        {
            TemplateName = templateName;
        }

        public override string TemplateName
        {
            get;
        }
    }
}