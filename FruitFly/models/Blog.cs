// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractTemplate
    {
        public Blog() : base(null)
        {
        }

        public override string TemplateName => 
            Global.TEMPLATE_INDEX;

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string GetVariableValue(Variable variable)
        {
            if(variable.Scope == Global.SCOPE_NAME_BLOG && variable.Name == Global.VAR_NAME_INDEX_POSTS)
            {
                return RenderPostTiles();
            }

            if(Parent != null)
            {
                return Parent.GetVariableValue(variable);
            }

            return base.GetVariableValue(variable);
        }

        private string RenderPostTiles()
        {
            var sb = new StringBuilder();
            foreach (var post in Posts)
            {
                sb.Append(post.Render(RenderedFormats.Html, Global.MORPH_TILE));
            }
            return sb.ToString();
        }
    }
}