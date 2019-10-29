using System;
using System.Collections.Generic;
using System.IO;

namespace fruitfly.objects
{
    public class HtmlRenderer
    {
        public HtmlRenderer(Context context)
        {
            Context = context;
        }

        internal string RenderBlog(Blog blog)
        {
            return Context.Binder.BindVariables(
                Context.Storage.LoadContent(Global.INDEX_HTML)
            );
        }

        public string RenderPostAsJumbotron(Post post)
        {
            return "TODO Jumbo";
        }

        public string RenderPost(Post post)
        {
            return Context.Binder.BindVariables(
                Context.Storage.LoadContent(Global.POST_HTML),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_CONTENT, () => MdConverter.Convert(File.ReadAllText(post.ArticleFileInfo.FullName)) }
                }
            );
        }

        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();

        public Context Context { get; }
    }
}