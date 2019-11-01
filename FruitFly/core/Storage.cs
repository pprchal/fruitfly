// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.IO;
using fruitfly.objects;

namespace fruitfly.core
{
    public class Storage : BaseLogic
    {
        public string LoadTemplate(string templateName)
        {
            return File.ReadAllText(Path.Combine(Context.Config.rootDir, Global.TEMPLATES, Context.Config.template, templateName));
        }

        internal void WriteContent(List<string> folderStack, string name, string content)
        {
            File.WriteAllText(
                CreateFullPath(folderStack, name),
                content
            );
        }

        private string CreateFullPath(List<string> folderStack, string name)
        {
            var outDirName = Path.Combine(
                Context.Config.rootDir,
                Global.BLOG_OUTPUT,
                Path.Combine(folderStack.ToArray())
            );

            if(Context.Config.rootDir != "" && !Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }
            
            return Path.Combine(outDirName, name);
        }
    }
}