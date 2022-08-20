// Pavel Prchal, 2019

namespace fruitfly.objects
{
    public interface IConfiguration
    {
        string workDir { get; }
        string language { get; }
        string home { get; }
        string title { get; }
        string template { get; }
        string fullVersion { get; }
        string templateDir { get; }
    }
}