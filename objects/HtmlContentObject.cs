namespace fruitfly.objects
{
    public abstract class HtmlContentObject
    {
        private HtmlContentObject()
        {
        }

        protected HtmlContentObject(Context context)
        {
            Context = context;
        }

        public abstract string Html
        {
            get;
        }

        public Context Context
        {
            get;
        }
    }
}