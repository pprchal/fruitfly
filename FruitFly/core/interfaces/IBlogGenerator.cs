// Pavel Prchal, 2019

using fruitfly.objects;

namespace fruitfly
{
    public interface IBlogGenerator
    {
        Blog GenerateBlog(string[] args);
    }
}