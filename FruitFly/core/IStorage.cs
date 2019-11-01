// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.objects;

namespace fruitfly.core
{
    public interface IStorage
    {
        string LoadTemplate(string templateName);

        void WriteContent(List<string> folderStack, string name, string content);

        Blog Scan(); 
    }
}