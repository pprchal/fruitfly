// Pavel Prchal, 2019

using Markdig;

namespace fruitfly
{
    public class MarkdigHtmlConverter : IMdConverter
    {
        private MarkdownPipeline pipeline = null;

        public MarkdigHtmlConverter()
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseBootstrap()
                .Build();
        }

        string IMdConverter.Convert(string mdContent)
        {
            return Markdown.ToHtml(mdContent, pipeline);
        }
    }
}