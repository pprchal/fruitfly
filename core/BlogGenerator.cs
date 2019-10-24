using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
            RenderIndex();
            ProcessBlog(Context);
        }

        private void RenderIndex()
        {
            WriteHtmlContent(BindVariables(File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, Global.INDEX_HTML))));
        }

        private void WriteHtmlContent(string htmlContent)
        {
            File.WriteAllText(Path.Combine(Global.BLOG_OUTPUT, Global.INDEX_HTML), htmlContent);
        }

        private string BindVariables(string content, Dictionary<string, Func<string>> actions = null)
        {
            return 
                BindCustomActions(
                    BindConfigVariables(new StringBuilder(content)),
                    actions
                ).ToString();
        }

        private StringBuilder BindCustomActions(StringBuilder sb, Dictionary<string, Func<string>> actions)
        {
            if(actions == null)
            {
                return sb;
            }

            foreach(var kvp in actions)
            {
                sb.Replace(
                    $"{{:{kvp.Key}}}",  // <div>{:content}</div>
                    kvp.Value.Invoke()
                );
            }

            return sb;
        }

        private StringBuilder BindConfigVariables(StringBuilder sb)
        {
            var t = typeof(Configuration);
            foreach(var propertyInfo in t.GetProperties())
            {
                sb.Replace(
                    $"{{{Global.VAR_NAME_CONFIG}:{propertyInfo.Name}}}",  // <title>{config:title}</title>
                    (string) t.InvokeMember(propertyInfo.Name, System.Reflection.BindingFlags.GetProperty, null, Context.Config, null)
                );
            }
            return sb;
        }

        private void ProcessBlog(Context context)
        {
            foreach(var year in Directory.EnumerateDirectories(Global.BLOG_INPUT))
            {
                ProcessYearDirectory(new DirectoryInfo(year));
            }
        }

        private void ProcessYearDirectory(DirectoryInfo year)
        {
            foreach(var month in year.EnumerateDirectories())
            {
                ProcessMonthDirectory(year, month);
            }
        }


        private void ProcessMonthDirectory(DirectoryInfo year, DirectoryInfo month)
        {
            foreach(var contentDir in month.EnumerateDirectories())
            {
                System.Console.Out.WriteLine($"\t~o~ {year.Name}-{month.Name}");
                ProcessContentDirectory(year, month, contentDir);
            }
        }

        private void ProcessContentDirectory(DirectoryInfo year, DirectoryInfo month, DirectoryInfo contentDir)
        {
            var content = ContentDir.Parse(contentDir);
            foreach(var fileInfo in contentDir.EnumerateFiles())
            {
                if(IsTemplateContentFile(fileInfo))
                {
                    RenderFileByTemplate(year, month, content, fileInfo);
                }
                else
                {
                    // CopyUntouched(day);
                }
            }
        }

        private bool IsTemplateContentFile(FileInfo fileInfo)
        {
            return fileInfo.FullName.EndsWith(".md");
        }

        private string RenderFileByTemplate(DirectoryInfo year, DirectoryInfo month, ContentDir contentDir, FileInfo singleContentFileInfo)
        {
            var renderedPost = BindVariables(
                File.ReadAllText(Path.Combine(Global.TEMPLATES, Context.Config.template, Global.POST_HTML)),
                new Dictionary<string, Func<string>>()
                {
                    { Global.VAR_NAME_CONTENT, () => MdConverter.Convert(File.ReadAllText(singleContentFileInfo.FullName)) }
                }
            );

            File.WriteAllText(
                GetOutFileNameAndEnsureDir(year, month, contentDir, singleContentFileInfo),
                renderedPost
            );

            return renderedPost;
        }

        private string GetOutFileNameAndEnsureDir(DirectoryInfo year, DirectoryInfo month, ContentDir contentDir, FileInfo singleContentFileInfo)
        {
            var dirName = Path.Combine(Global.BLOG_OUTPUT, year.Name, month.Name, contentDir.Name);
            if(!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            return Path.Combine(dirName, singleContentFileInfo.Name + ".html");
        }

        private void CopyUntouched(FileInfo fileInfo)
        {
            // var outFileName = Path.Combine(Global.BLOG_OUTPUT
            // File.Copy(fileInfo.FullName, outFileName);
        }

        sealed internal class ContentDir
        {
            public int Day;
            public int Number;
            private static Regex TemplateRe = new Regex("^d([0-9]+)_post([0-9]+)", RegexOptions.Compiled);

            public string Name { get; internal set; }

            public static ContentDir Parse(DirectoryInfo contentDirInfo)
            {
                var m = TemplateRe.Match(contentDirInfo.Name);
                if(m.Success)
                {
                    return new ContentDir()
                    {
                        Name = contentDirInfo.Name,
                        Day = Convert.ToInt32(m.Groups[1].Value),
                        Number = Convert.ToInt32(m.Groups[2].Value)
                    };
                }

                return null;
            }        
        }


        IMdConverter MdConverter
        {
            get;
        } = new MarkdigHtmlConverter();
    }
}