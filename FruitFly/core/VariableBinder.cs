// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fruitfly.core
{
    public class VariableBinder 
    {
        static readonly Regex VariableRegex = new("\\{([\\w\\d]+):([\\w\\.\\d]+)\\}", RegexOptions.Compiled);

        public async Task<string> Bind(string content, IVariableSource variableSource)
        {
            var sb = new StringBuilder(content);
            var variables = FindVariablesInContent(content);
            var n = variables.Count();

            foreach (var variable in variables)
            {
                var val = await variableSource.GetVariableValue(variable);
                sb.Replace(
                    variable.ReplaceBlock,
                    val
                );
            }
            return sb.ToString();
        }


        IEnumerable<Variable> FindVariablesInContent(string content) =>
            VariableRegex
                .Matches(content)
                .Select(match => Variable.CreateFrom(match));
    }
}