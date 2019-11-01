// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly.core
{
    public class VariableBinder : BaseLogic
    {
        static Regex VariableRegex => new Regex("\\{([\\w\\d]+):([\\w\\d]+)\\}", RegexOptions.Compiled);

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

        public string BindVariables(string content, Dictionary<string, Func<string>> actions = null)
        {
            return BindCustomActions(
                BindConfigVariables(new StringBuilder(content)),
                actions
            ).ToString();
        }

        private StringBuilder BindCustomActions(StringBuilder sb, Dictionary<string, Func<string>> actions)
        {
            if(actions == null)
            {
                return sb;
            }

            foreach(var kvp in actions)
            {
                sb.Replace(
                    $"{{:{kvp.Key}}}",  // <div>{:content}</div>
                    kvp.Value.Invoke()
                );
            }

            return sb;
        }        

        private StringBuilder BindConfigVariables(StringBuilder sb)
        {
            var t = typeof(Configuration);
            foreach(var propertyInfo in t.GetProperties())
            {
                sb.Replace(
                    $"{{{Global.VAR_NAME_CONFIG}:{propertyInfo.Name}}}",  // <title>{config:title}</title>
                    (string) t.InvokeMember(propertyInfo.Name, System.Reflection.BindingFlags.GetProperty, null, Context.Config, null)
                );
            }
            return sb;
        }        
    }
}