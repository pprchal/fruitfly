using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using fruitfly.objects;

namespace fruitfly
{
    public class BlogGenerator
    {
        public Context Context { get; }

        private BlogGenerator()
        {
        }

        public BlogGenerator(Context context)
        {
            Context = context;
        }

        public Blog GenerateBlog()
        {
            var blog = new BlogScanner(Context).Scan(Global.BLOG_INPUT);
            RenderBlogPosts(blog);
            RenderIndex(blog);
            return blog;
        }

        private void RenderIndex(Blog blog)
        {
            var htmlContent = new VariableBinder(Context).BindVariables(
                File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, Global.INDEX_HTML))
            );
            File.WriteAllText(Path.Combine(Global.BLOG_OUTPUT, Global.INDEX_HTML), htmlContent);
        }

        private void RenderBlogPosts(Blog blog)
        {
            var sb = new StringBuilder();
            foreach(var post in blog.Posts)
            {
                sb.Append(HtmlRenderer.RenderPostAsJumbotron(post));
                RenderPost(post);
            }
        }

        private string RenderPost(Post post)
        {
            var renderedPost = new VariableBinder(Context).BindVariables(
                File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, Global.POST_HTML)),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_CONTENT, () => MdConverter.Convert(File.ReadAllText(post.ArticleFileInfo.FullName)) }
                }
            );

            File.WriteAllText(
                GetOutFileNameAndEnsureDir(post),
                renderedPost
            );

            return renderedPost;
        }

        private string GetOutFileNameAndEnsureDir(Post post)
        {
            var outDirName = post.Name.Replace(Global.BLOG_INPUT + "\\", Global.BLOG_OUTPUT + "\\");
            if(!Directory.Exists(outDirName))
            {
                Directory.CreateDirectory(outDirName);
            }

            return Path.Combine(outDirName, post.ArticleFileInfo.Name + ".html");
        }

        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();


     
    }
}