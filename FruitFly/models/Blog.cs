// Pavel Prchal, 2019, 2023

using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // var sb = new StringBuilder();
            // foreach (var post in Posts)
            // {
            //     var tile = await post.Render(Constants.MORPH_TILE);
            //     sb.Append(tile);
            // }


            var pr = Posts.Select(async post => await post.Render(Constants.MORPH_TILE));
            var x = await Task.WhenAll(pr);

            return string.Join(" ", x);

            // return sb.ToString();
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