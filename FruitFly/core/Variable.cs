// Pavel Prchal, 2019

using System.Text.RegularExpressions;

namespace fruitfly
{
    public readonly struct Variable(string name, string scope, string replaceBlock)
    {
        public readonly string Name = name;
        public readonly string Scope = scope;
        public readonly string ReplaceBlock = replaceBlock;

        public override string ToString() => $"[{Scope}::{Name}]";

        internal static Variable CreateFrom(Match match) =>
            new(
                name: match.Groups[2].Value,
                scope: match.Groups[1].Value,
                replaceBlock: match.Groups[0].Value
            );
    }
}