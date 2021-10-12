// Pavel Prchal, 2019

using System.Threading.Tasks;

namespace fruitfly.core
{
    public interface IVariableSource
    {
        Task<string> GetVariableValue(Variable variable);
    }
}
