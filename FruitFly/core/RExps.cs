// Pavel Prchal, 2024

using System.Text.RegularExpressions;

namespace fruitfly
{
    public partial class RExps 
    {
        public static readonly Regex VARIABLE = VariableRegex_();
        [GeneratedRegex("\\{([\\w\\d]+):([\\w\\.\\d]+)\\}", RegexOptions.Compiled)]
        private static partial Regex VariableRegex_();

        public static readonly Regex DIRECTORY_WIN = DirectoryWindows_();
        [GeneratedRegex(@"y(\d+)\\m(\d+)\\d([\d+]+)_post([\d+]+$)", RegexOptions.Compiled)]
        private static partial Regex DirectoryWindows_();

        public static readonly Regex DIRECTORY_UNIX = DirectoryUnix_();
        [GeneratedRegex(@"y(\d+)\/m(\d+)\/d([\d+]+)_post([\d+]+$)", RegexOptions.Compiled)]
        private static partial Regex DirectoryUnix_();

    }
}