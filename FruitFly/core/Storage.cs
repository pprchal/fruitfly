// Pavel Prchal, 2019

using System;
using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class Storage : BaseLogic
    {
        public string LoadTemplate(TemplateItems templateItem)
        {
            switch (templateItem)
            {
                case TemplateItems.Index: return LoadContent(Global.TEMPLATE_INDEX);
                case TemplateItems.Post:  return LoadContent(Global.TEMPLATE_POST);
                case TemplateItems.PostTile: return LoadContent(Global.TEMPLATE_POST_TILE);
            }

            throw new Exception($"Unknown template: [{templateItem}]");
        }

        public string LoadContent(string contentName)
        {
            return File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, contentName));
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