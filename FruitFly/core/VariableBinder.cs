// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fruitfly
{
    public class VariableBinder 
    {
        public async Task<string> Bind(string content, IVariableSource variableSource)
        {
            var sb = new StringBuilder(content);

            foreach (var variable in FindVariablesInContent(content))
            {
                sb.Replace(
                    variable.ReplaceBlock,
                    await variableSource.GetVariableValue(variable)
                );
            }
            return sb.ToString();
        }


        IEnumerable<Variable> FindVariablesInContent(string content) =>
            RExps.VARIABLE
                .Matches(content)
                .Select(Variable.CreateFrom);
    }
}