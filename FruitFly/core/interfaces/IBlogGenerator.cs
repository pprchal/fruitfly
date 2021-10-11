// Pavel Prchal, 2019

using System.Threading.Tasks;
using fruitfly.objects;

namespace fruitfly
{
    public interface IBlogGenerator
    {
        Task<Blog> GenerateBlogAsync();
    }
}