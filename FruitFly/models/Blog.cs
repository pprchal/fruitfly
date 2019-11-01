// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractContentObject
    {
        public Blog(Context context, AbstractContentObject parent) : base(context, parent)
        {
        }

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string GetVariableValue(Variable variable) => variable switch
        {
            { Scope: "config" } => (Context.Config as IVariableSource).GetVariableValue(variable),
            { Scope: "blog", Name: Global.VAR_NAME_INDEX_POSTS } => RenderPostTiles(),
            _ => throw new System.Exception($"GetVariableValue {variable.ReplaceBlock}")
        };

        private string RenderPostTiles()
        {
            var sb = new StringBuilder();
            foreach (var post in Posts)
            {
                sb.Append(Context.GetLogic<HtmlRenderer>().RenderTemplate(Global.TEMPLATE_POST_TILE, post));
            }
            return sb.ToString();
        }
    }
}