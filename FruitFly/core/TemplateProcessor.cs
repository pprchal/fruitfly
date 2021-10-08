// Pavel Prchal, 2019, 2020

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace fruitfly.core
{
    public static class TemplateProcessor 
    {
        public static string Process(string content, IVariableSource variableSource, string diag) =>
            FindVariablesIn(content)
                .Aggregate(
                    new StringBuilder(content),
                    (sb, variable) =>
                    {
                        var value = variableSource.GetVariableValue(variable);
                        Context.ConsoleWrite($"[{diag}]:{variable.ReplaceBlock} <= {value}");
                        sb.Replace(
                            variable.ReplaceBlock,
                            value
                        );
                        return sb;
                    }
            ).ToString();

        static readonly Regex VARIABLE_PATTERN = new Regex(@"\{([\w\d]*):([\w\.\d]+)\}", RegexOptions.Compiled);

        private static IEnumerable<Variable> FindVariablesIn(string content) =>
            from match in VARIABLE_PATTERN.Matches(content).OfType<Match>()
            select Variable.CreateFrom(match);
    }
}