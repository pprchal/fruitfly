// Pavel Prchal, 2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fruitfly.core;

namespace fruitfly.objects
{
    public class Blog : AbstractTemplate, IStorageContent, IVariableSource
    {
        private readonly StringBuilder sb = new StringBuilder();

        public override string Render(RenderedFormats format, string morph = null)
        {
            var html = TemplateProcessor.Process(
                content: Context.Storage.LoadTemplate(Constants.Templates.Index),
                variableSource: this,
                diag: Constants.Templates.Index
            );

            return html;
        }

        public Blog() : base(null)
        {
        }

        public double EllapsedSeconds =>
            new TimeSpan(DateTime.Now.Ticks - Context.StartTime.Ticks).TotalSeconds;


        public override string TemplateName => 
            Constants.Templates.Index;

        public IEnumerable<Post> Posts
        {
            get;
            internal set;
        }

        public override string GetVariableValue(Variable variable)
        {
            if(variable.Scope == Constants.Blog.Scope && 
               variable.Name == Constants.Blog.Posts)
            {
                var postTiles = Posts.Aggregate(
                    sb.Clear(),
                    (sb, post) => sb.Append(post.Render(RenderedFormats.Html, Constants.Blog.Tile))
                ).ToString();  
                return postTiles;          
            }
                
            if(Parent != null)
            {
                return (Parent as IVariableSource).GetVariableValue(variable);
            }

            return base.GetVariableValue(variable);
        }

        string[] IStorageContent.BuildFolderStack() =>
            new string[] { "index.html" };
    }
}