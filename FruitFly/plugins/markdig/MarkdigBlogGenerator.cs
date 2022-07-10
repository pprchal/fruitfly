// Pavel Prchal, 2019

using Markdig;

namespace fruitfly
{
    public class MarkdigHtmlConverter : IConverter
    {
        readonly MarkdownPipeline Pipeline;

        public MarkdigHtmlConverter()
        {
            Pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .UseDiagrams()
                .UseEmojiAndSmiley()
                .UseGlobalization()
                .UseMediaLinks()
                .UseBootstrap()
                .UseFooters()
                .Build();
        }

        string IConverter.Convert(string mdContent) =>
            Markdown.ToHtml(mdContent, Pipeline);
    }
}