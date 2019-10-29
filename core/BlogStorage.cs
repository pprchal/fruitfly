using System.IO;

namespace fruitfly
{
    public class BlogStorage
    {
        private Context Context;

        public BlogStorage(Context context)
        {
            this.Context = context;
        }

        public string LoadContent(string contentName)
        {
            var fullFileName = "";

            if(contentName == Global.INDEX_HTML)
            {
                fullFileName = Path.Combine(Global.TEMPLATES, Context.Config.template, Global.INDEX_HTML);            
            }
            else if(contentName == Global.POST_HTML)
            {
                fullFileName = Path.Combine(Global.TEMPLATES, Context.Config.template, Global.POST_HTML);         
            }   

            return File.ReadAllText(fullFileName);
        }
    }
}