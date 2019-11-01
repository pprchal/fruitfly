// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractContentObject
    {
        private readonly Context Context;

        public Blog(AbstractContentObject parent, Context context) : base(parent)
        {
            Context = context;
        }

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string GetVariableValue(Variable variable) => variable.Name switch
        {
            Global.VAR_NAME_INDEX_POSTS => RenderPostRows(this),
            _ => (Context.Config as IVariableSource).GetVariableValue(variable)
            // throw new Exception($"Cannot resolve variable[{variable.ReplaceBlock}] on post[{File.FullName}]")
        };

        private string RenderPostRows(Blog blog)
        {
            var sb = new StringBuilder();
            foreach (var post in blog.Posts)
            {
                sb.Append(Context.GetLogic<HtmlRenderer>().Render(post, Templates.PostTile));
            }
            return sb.ToString();
        }
    }
}