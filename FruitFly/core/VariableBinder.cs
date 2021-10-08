// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace fruitfly.core
{
    public class VariableBinder 
    {
        static readonly Regex VariableRegex = new Regex("\\{([\\w\\d]+):([\\w\\.\\d]+)\\}", RegexOptions.Compiled);

        public StringBuilder Bind(string content, IVariableSource variableSource) =>
            FindVariablesInContent(content)
            .Aggregate(
                seed: new StringBuilder(content),
                func: (sb, variable) =>
                {
                    sb.Replace(
                        variable.ReplaceBlock,
                        variableSource.GetVariableValue(variable)
                    );
                    return sb;
                }
            );


        IEnumerable<Variable> FindVariablesInContent(string content) =>
            VariableRegex
                .Matches(content)
                .Select(match => Variable.CreateFrom(match));
    }
}