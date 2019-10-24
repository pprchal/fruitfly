using System;
using System.Collections.Generic;
using System.IO;
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

        public void GenerateBlog()
        {
            var blog = new BlogScanner().Scan(Global.BLOG_INPUT);
            RenderBlogPosts(blog);
            RenderIndex();
        }

        private void RenderIndex()
        {
            WriteHtmlContent(
                new VariableBinder(Context).BindVariables(File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, Global.INDEX_HTML)))
            );
        }

        private void WriteHtmlContent(string htmlContent)
        {
            File.WriteAllText(Path.Combine(Global.BLOG_OUTPUT, Global.INDEX_HTML), htmlContent);
        }

        private void RenderBlogPosts(Blog blog)
        {
            foreach(var post in blog.Posts)
            {
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

            // File.WriteAllText(
            //     GetOutFileNameAndEnsureDir(year, month, contentDir, singleContentFileInfo),
            //     renderedPost
            // );

            return renderedPost;
        }

        // private string GetOutFileNameAndEnsureDir(DirectoryInfo year, DirectoryInfo month, ContentDir contentDir, FileInfo singleContentFileInfo)
        // {
        //     var dirName = Path.Combine(Global.BLOG_OUTPUT, year.Name, month.Name, contentDir.Name);
        //     if(!Directory.Exists(dirName))
        //     {
        //         Directory.CreateDirectory(dirName);
        //     }

        //     return Path.Combine(dirName, singleContentFileInfo.Name + ".html");
        // }

        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();


// <div class="jumbotron">
//   <h1 class="display-4">{post:title}</h1>
//   <p class="lead">{post:body}</p>
//   <hr class="my-4">
//   <p>It uses utility classes for typography and spacing to space content out within the larger container.</p>
//   <a class="btn btn-primary btn-lg" href="#" role="button">Learn more</a>
// </div>        
    }
}