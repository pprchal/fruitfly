// Pavel Prchal, 2019

using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class BlogStorage : BaseLogic
    {

        public string LoadContent(TemplateItems templateItem)
        {
            var fullFileName = "";

            switch (templateItem)
            {
                case TemplateItems.Index:
                    fullFileName = Path.Combine(Global.TEMPLATES, Context.Config.template, Global.TEMPLATE_INDEX);
                    break;
                case TemplateItems.Post:
                    fullFileName = Path.Combine(Global.TEMPLATES, Context.Config.template, Global.TEMPLATE_POST);
                    break;
                case TemplateItems.PostRow:
                    fullFileName = Path.Combine(Global.TEMPLATES, Context.Config.template, Global.TEMPLATE_POST_TILE);
                    break;
            }

            return File.ReadAllText(fullFileName);
        }

        internal void WriteContent(TemplateItems templateItem, string content, Post post = null)
        {
            switch (templateItem)
            {
                case TemplateItems.Index:
                    File.WriteAllText(Path.Combine(Global.BLOG_OUTPUT, Global.TEMPLATE_INDEX), content);
                    break;
                case TemplateItems.Post:
                    File.WriteAllText(
                        GetOutFileNameAndEnsureDir(post),
                        content
                    );
                    break;
            }
        }

        private string GetOutFileNameAndEnsureDir(Post post)
        {
            var outDirName = post.Name.Replace(Global.BLOG_INPUT + "\\", Global.BLOG_OUTPUT + "\\");
            if(!Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }

            return Path.Combine(outDirName, post.File.Name + ".html");
        }        
    }
}