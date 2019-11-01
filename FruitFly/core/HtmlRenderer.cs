// Pavel Prchal, 2019

using fruitfly.objects;

namespace fruitfly.core
{
    public class HtmlRenderer : BaseLogic
    {
        public string Render(AbstractContentObject contentObject, Templates? templateItem = null)
        {
            if(contentObject is Blog)
            {
                return RenderInternal(contentObject, Templates.Index);
            }
            else if (templateItem != null && templateItem.Value == Templates.PostTile)
            {
                return RenderInternal(contentObject, Templates.PostTile);
            }
            else if (contentObject is Post)
            {
                return RenderInternal(contentObject, Templates.Post);
            }

            return "";
        }

        private string RenderInternal(AbstractContentObject contentObject, Templates template)
        {
            return Context.GetLogic<VariableBinder>().Bind(
                Context.GetLogic<Storage>().LoadTemplate(template),
                contentObject
            ).ToString();
        }
   }
}