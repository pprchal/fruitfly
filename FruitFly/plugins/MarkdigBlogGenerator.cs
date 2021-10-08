// Pavel Prchal, 2019

using fruitfly.core;
using Markdig;

namespace fruitfly
{
    public class MarkdigHtmlConverter : IMdConverter
    {
        private readonly MarkdownPipeline pipeline = null;

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

        string IMdConverter.Convert(string mdContent) =>
            Markdown.ToHtml(mdContent, pipeline);
    }
}