// Pavel Prchal, 2019

using fruitfly.objects;
using System.IO;

namespace fruitfly.core
{
    public interface IContext
    {
        T GetLogic<T>() where T : BaseLogic, new();

        Configuration CreateConfig();
    }
}