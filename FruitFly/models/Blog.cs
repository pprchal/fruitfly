// Pavel Prchal, 2019, 2023

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fruitfly
{
    public class Blog : AbstractTemplate
    {

        public override async Task<string> Render(string morph = null) =>
            await base.Render(morph);

        public override string TemplateName => 
            Constants.Templates.INDEX;

        public readonly IList<Post> Posts = new List<Post>();

        public override Task<string> GetVariableValue(Variable variable)
        {
            if(variable.Scope == Constants.Scope.BLOG && 
               variable.Name == Constants.Variables.INDEX_POSTS)
            {
                return RenderPostTiles();
            }

            if(Parent != null)
            {
                return Parent.GetVariableValue(variable);
            }

            return base.GetVariableValue(variable);
        }

        async Task<string> RenderPostTiles()
        {
            var renderTasks = Posts.Select(async post => await post.Render(Constants.MORPH_TILE));
            return string.Join(" ", await Task.WhenAll(renderTasks));
        }

        internal static Blog Error(string msg)
        {
            return new Blog();
        }

        internal void AddPost(Post post)
        {
            Posts.Add(post);
            post.SetParent(this);
        }
    }
}