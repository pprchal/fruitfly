// Pavel Prchal, 2019

using System.Linq;
using System.Collections.Generic;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractTemplate
    {
        IConverter Converter;
        public Blog(IStorage storage) : base(storage)
        {
        }

        public override string Render(IConverter converter, string morph = null) 
        {
            Converter = converter;
            return base.Render(converter, morph);
        }

        public override string TemplateName => 
            Constants.Templates.INDEX;

        public IEnumerable<Post> Posts
        {
            get;
            set;
        }

        public override string GetVariableValue(Variable variable)
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

        string RenderPostTiles() =>
            Posts.Aggregate(
                seed: new StringBuilder(),
                func: (sb, post) =>
                {
                    sb.Append(
                        post.Render(
                            converter: Converter, 
                            morph: Constants.MORPH_TILE
                        )
                    );
                    return sb;
                }
            ).ToString();
    }
}