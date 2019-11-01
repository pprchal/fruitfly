// Pavel Prchal, 2019

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace fruitfly.core
{
    public class VariableBinder : BaseLogic
    {
        static Regex VariableRegex => new Regex("\\{([\\w\\d]+):([\\w\\.\\d]+)\\}", RegexOptions.Compiled);

        public StringBuilder Bind(string content, IVariableSource variableSource)
        {
            var sb = new StringBuilder(content);

            foreach (var variable in FindVariables(content))
            {
                sb.Replace(
                    variable.ReplaceBlock,
                    variableSource.GetVariableValue(variable)
                );
            }
            
            return sb;
        }

        private List<Variable> FindVariables(string content)
        {
            var variables = new List<Variable>();
            foreach (Match match in VariableRegex.Matches(content))
            {
                variables.Add(Variable.CreateFrom(match));
            }

            return variables;
        }
    }
}