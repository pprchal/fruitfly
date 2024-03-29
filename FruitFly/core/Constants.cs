// Pavel Prchal, 2019

namespace fruitfly
{
    public static class Constants
    {
        public const string BLOG_INPUT = "blog_input";
        public const string MORPH_TILE = "tile";
        public const string VAR_NAME_CONTENT = "content";

        public static class Templates
        {
            public const string FOLDER = "templates";
            public const string INDEX = "index.html";
            public const string POST = "post.html";
            public const string POST_TILE = "postTile.html";
        }
        public class Config
        {
            public const string YML = "config.yml";
        }        

        public static class Scope
        {
            public const string CONFIG = "config";

            public const string TEMPLATE = "template";

            public const string BLOG = "blog";

            public const string POST = "post";
        }

        // blog:posts
        public static class Variables
        {
            public const string INDEX_POSTS = "posts";
            public const string POST_TITLE = "title";
            public const string POST_TITLE_TILE = "titleTile";
            public const string POST_CREATED = "created";
            public const string POST_URL = "url";
            public const string POST_CONTENT = "content";
        }
    }
}