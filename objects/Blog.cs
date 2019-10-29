using System.Collections.Generic;

namespace fruitfly.objects
{
    public class Blog : HtmlContentObject, IVariableProvider
    {
        public Blog(IVariableProvider parent) : base(parent)
        {
        }

        public List<Post> Posts
        {
            get;
        } = new List<Post>();

        public override string Html => HtmlRenderer.Render(this);

        string IVariableProvider.GetVariableValue(string name)
        {
            return "~o~";
        }
    }
}