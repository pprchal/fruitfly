// Pavel Prchal, 2019

using System.Collections.Generic;
using fruitfly.objects;

namespace fruitfly.core
{
    public interface IStorage
    {
        string LoadTemplate(string templateName);

        void WriteContent(List<string> folderStack, string name, RenderedFormats formats, string content);

        string LoadByStorageId(string storageId);

        Blog Scan(); 
    }
}