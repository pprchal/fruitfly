// Pavel Prchal, 2019

using System.Collections.Generic;

namespace fruitfly.objects
{
    public class Blog : AbstractContentObject
    {
        public List<Post> Posts
        {
            get;
        } = new List<Post>();


        public override string GetVariableValue(string name) 
        {
            return ":aaaaa";
        }        
    }
}