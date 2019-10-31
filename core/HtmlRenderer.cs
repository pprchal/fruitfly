// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using fruitfly.objects;

namespace fruitfly.core
{
    public class HtmlRenderer : BaseLogic
    {
        public string Render(AbstractContentObject contentObject, TemplateItems? templateItem = null)
        {
            if(contentObject is Blog)
            {
                return RenderIndex(contentObject as Blog);
            }
            else if(contentObject is Post)
            {
                return RenderPost(contentObject as Post);
            }
            else if(contentObject is Post && templateItem != null && templateItem.Value == TemplateItems.PostTile)
            {
                return RenderPostTile(contentObject as Post);
            }

            return "";
        }

        private string RenderIndex(Blog blog)
        {
            return Context.GetLogic<VariableBinder>().BindVariables(
                Context.GetLogic<BlogStorage>().LoadTemplate(TemplateItems.Index),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_INDEX_POSTS, () => RenderPostRows(blog) }
                }
            );
        }

        private string RenderPost(Post post)
        {
            return Context.GetLogic<VariableBinder>().BindVariables(
                Context.GetLogic<BlogStorage>().LoadTemplate(TemplateItems.Post),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_CONTENT, () => MdConverter.Convert(File.ReadAllText(post.File.FullName)) }
                }
            );
        }

        private string RenderPostTile(Post post)
        {
            return Context.GetLogic<VariableBinder>().BindVariables(
                Context.GetLogic<BlogStorage>().LoadTemplate(TemplateItems.PostTile),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_POST_TITLE, () => post.Title },
                    { Global.VAR_NAME_POST_TITLE_TILE, () => post.TitleTile },
                    { Global.VAR_NAME_POST_CREATED, () => Context.GetLogic<HtmlRenderer>().ToLocaleDate(post.Created) },
                    { Global.VAR_NAME_POST_URL, () => post.Url }
                }
            );
        }

        private string RenderPostRows(Blog blog)
        {
            var sb = new StringBuilder();
            foreach(var post in blog.Posts)
            {
                sb.Append(RenderPostTile(post));
            }
            return sb.ToString();
        }


        private CultureInfo _Culture = null;
        CultureInfo Culture
        {
            get
            {
                if(_Culture == null)
                {
                    _Culture = new CultureInfo(Context.Config.language.Replace("_", "-"));
                }
                return _Culture;
            }
        }
        
        private string ToLocaleDate(DateTime date)
        {
            return date.ToString("d", Culture);
        }

        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();


        public Content LoadContentPart(string contentName)
        {
            var template = Context.GetLogic<BlogStorage>().LoadContent(contentName);

            var obj = new Content()
            {
                Html = template
            };

            return obj;

        }

        public void TTT()
        {
            var c = LoadContentPart("menu.html");
        }
   }
}