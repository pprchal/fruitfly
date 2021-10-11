// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using fruitfly.core;
using System.Threading.Tasks;

namespace fruitfly.objects
{
    public class Blog : AbstractTemplate
    {
        IConverter Converter;
        public Blog(IStorage storage) : base(storage)
        {
        }

        public override async Task<string> Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return await base.Render(converter, morph);
        }

        public override string TemplateName => 
            Constants.Templates.INDEX;

        public IEnumerable<Post> Posts
        {
            get;
            set;
        }

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
            var sb = new StringBuilder();
            foreach (var post in Posts)
            {
                var tile = await post.Render(
                    converter: Converter,
                    morph: Constants.MORPH_TILE
                );

                sb.Append(tile);
            }
            return sb.ToString();
        }
    }
}