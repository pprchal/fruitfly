// Pavel Prchal, 2019

using fruitfly.objects;

namespace fruitfly.core
{
    public class HtmlRenderer : BaseLogic
    {
        internal string RenderTemplate(string templateName, AbstractContentObject contentObject)
        {
            return Context.GetLogic<VariableBinder>().Bind(
                Context.GetLogic<Storage>().LoadTemplate(templateName),
                contentObject
            ).ToString();
        }
    }
}