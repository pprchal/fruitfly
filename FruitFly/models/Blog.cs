// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractTemplate
    {
        public Blog(Context context, AbstractTemplate parent) : base(context, parent)
        {
        }

        public override string TemplateName => Global.TEMPLATE_INDEX;

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string GetVariableValue(Variable variable) => variable switch
        {
            { Scope: Global.SCOPE_NAME_TEMPLATE } => RenderNestedTemplateByVariable(variable), 
            { Scope: Global.SCOPE_NAME_CONFIG } => (Context.Config as IVariableSource).GetVariableValue(variable),
            { Scope: Global.SCOPE_NAME_BLOG, Name: Global.VAR_NAME_INDEX_POSTS } => RenderPostTiles(),
            _ => throw new System.Exception($"GetVariableValue {variable.ReplaceBlock}")
        };

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