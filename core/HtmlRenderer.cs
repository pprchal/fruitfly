using System;

namespace fruitfly.objects
{
    internal class HtmlRenderer
    {
        internal static string Render(Blog blog)
        {
            throw new NotImplementedException();
        }

        internal static string RenderPostAsJumbotron(Post post)
        {
            return "x";
            // return $"<div class=\"jumbotron\"><h1 class=\"display-4\">{post.Title}</h1><p class=\"lead\">{post.HtmlShort}</p><hr class=\"my-4\"><p>It uses utility classes for typography and spacing to space content out within the larger container.</p><a class=\"btn btn-primary btn-lg\" href=\"#\" role=\"button\">Learn more</a></div>";
        }

        internal static string RenderPost(Post post)
        {
            throw new NotImplementedException();
        }

        internal static string RenderDate(DateTime created)
        {
            throw new NotImplementedException();
        }
    }
}