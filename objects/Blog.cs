// Pavel Prchal, 2019

using System.Collections.Generic;

namespace fruitfly.objects
{
    public class Blog : ContentObject
    {
        public List<Post> Posts
        {
            get;
        } = new List<Post>();
    }
}