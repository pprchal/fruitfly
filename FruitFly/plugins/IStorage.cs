// Pavel Prchal, 2019

using fruitfly.objects;

namespace fruitfly.core
{
    public interface IStorage
    {
        string LoadTemplate(string templateName);

        void WriteContent(string[] folderStack, string name, string content);

        string LoadContentByStorageId(string storageId);

        Blog Scan(); 
    }
}