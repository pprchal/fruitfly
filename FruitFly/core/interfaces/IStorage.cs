// Pavel Prchal, 2019

using System.Threading.Tasks;
using fruitfly.objects;

namespace fruitfly.core
{
    public interface IStorage
    {
        string LoadTemplate(string templateName);

        Task WriteContent(IStorageContent folderStack, string name, string content);

        string LoadContent(string storageId);

        Blog Scan(); 
    }
}