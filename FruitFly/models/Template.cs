// Pavel Prchal, 2019

namespace fruitfly
{
    public class Template(AbstractTemplate parent, string templateName) : AbstractTemplate(parent)
    {
        public override string TemplateName
        {
            get;
        } = templateName;
    }
}