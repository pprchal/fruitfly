namespace fruitfly.objects
{
    public abstract class HtmlContentObject 
    {
        private HtmlContentObject()
        {
        }

        protected HtmlContentObject(IVariableProvider parent)
        {
            Parent = parent;
        }

        public abstract string Html
        {
            get;
        }

        protected IVariableProvider Parent
        {
            get;
        }
    }
}