// Pavel Prchal, 2019

using System;
using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class Storage : BaseLogic
    {
        public string LoadTemplate(Templates template)
        {
            switch (template)
            {
                case Templates.Index: return LoadTemplate(Global.TEMPLATE_INDEX);
                case Templates.Post:  return LoadTemplate(Global.TEMPLATE_POST);
                case Templates.PostTile: return LoadTemplate(Global.TEMPLATE_POST_TILE);
            }

            throw new Exception($"Unknown template: [{template}]");
        }

        public string LoadTemplate(string templateName)
        {
            return File.ReadAllText(Path.Combine(Context.Config.rootDir, Global.TEMPLATES, Context.Config.template, templateName));
        }

        internal void WriteContent(Templates template, string content, Post post = null)
        {
            switch (template)
            {
                case Templates.Index:
                    File.WriteAllText(Path.Combine(Context.Config.rootDir, Global.BLOG_OUTPUT, Global.TEMPLATE_INDEX), content);
                    break;
                case Templates.Post:
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