// Pavel Prchal, 2019


using System;
using System.Text.RegularExpressions;

namespace fruitfly.core
{
    public class Variable
    {
        public string Name;
        public string Scope;
        public string ReplaceBlock;

        internal static Variable CreateFrom(Match match)
        {
            return new Variable()
            {
                Scope = match.Groups[1].Value,
                Name = match.Groups[2].Value,
                ReplaceBlock = match.Groups[0].Value
            };
        }
    }
}