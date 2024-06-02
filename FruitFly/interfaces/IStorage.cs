// Pavel Prchal, 2019

using System.Threading.Tasks;

namespace fruitfly
{
    public interface IStorage
    {
        Task<string> LoadTemplate(string templateName);

        Task<string> LoadContentByStorageId(string storageId);

        Task<bool> WriteContent(string[] folderStack, string name, string content);

        Task<Blog> LoadBlog(); 
    }
}