// Pavel Prchal, 2019

using System.Text.RegularExpressions;

namespace fruitfly
{
    public sealed class Variable
    {
        Variable(string name, string scope, string replaceBlock)
        {
            Name = name;
            Scope = scope;
            ReplaceBlock = replaceBlock;
        }

        public readonly string Name;
        public readonly string Scope;
        public readonly string ReplaceBlock;

        public override string ToString() => $"[{Scope}::{Name}]";

        internal static Variable CreateFrom(Match match) =>
            new(
                name: match.Groups[2].Value,
                scope: match.Groups[1].Value,
                replaceBlock: match.Groups[0].Value
            );
    }
}