// Pavel Prchal, 2019

using fruitfly.objects;

namespace fruitfly.core
{
    public interface IContext
    {
        T GetLogic<T>() where T : AbstractLogic, new();

        Configuration CreateConfig();
    }
}