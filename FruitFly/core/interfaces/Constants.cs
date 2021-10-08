// Pavel Prchal, 2019, 2020

namespace fruitfly.core
{
    public static class Constants
    {
        public static class Templates
        {
            public const string Scope = "template";

            public const string Directory = "templates";
            public const string Index = "index.html";
            public const string Post = "post.html";
            public const string Tile = "postTile.html";
        }

        public class Config
        {
            public const string Scope = "config";

            public const string FileName = "config.yml";
        }

        public static class Blog
        {
            public const string Scope = "blog";

            public const string InputDirectory = "blog_input";
            public const string Posts = "posts";
            public const string Tile = "tile";
        }

        public static class Post
        {
            public const string Scope = "post";

            public const string Title = "title";
            public const string TitleTile = "titleTile";
            public const string Created = "created";
            public const string Url = "url";
            public const string Content = "content";
        }
    }
}