// Pavel Prchal, 2019

using Markdig;

namespace fruitfly
{
    public class MarkdigHtmlConverter : IConverter
    {
        private MarkdownPipeline pipeline = null;

        public MarkdigHtmlConverter()
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .UseDiagrams()
                .UseEmojiAndSmiley()
                .UseGlobalization()
                .UseMediaLinks()
                .UseBootstrap()
                .Build();
        }

        string IConverter.Convert(string mdContent) =>
            Markdown.ToHtml(mdContent, pipeline);
    }
}