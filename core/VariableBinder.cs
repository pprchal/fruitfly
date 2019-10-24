using System;
using System.Collections.Generic;
using System.Text;

namespace fruitfly
{
    public class VariableBinder
    {
        public Context Context { get; }

        private VariableBinder()
        {

        }

        public VariableBinder(Context context)
        {
            Context = context;
        }
        
        public string BindVariables(string input, Dictionary<string, Func<string>> actions = null)
        {
            return BindCustomActions(
                BindConfigVariables(new StringBuilder(input)),
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