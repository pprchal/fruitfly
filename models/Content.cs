// Pavel Prchal, 2019

namespace fruitfly.objects
{
    public class Content : AbstractContentObject
    {
        public override string GetVariableValue(string name) 
        {
            return ":aaaaa";
        }

        // public override string GetVariableValue(string name) => name switch
        // {
        //     Global.VAR_NAME_POST_TITLE => Title,
        //     Global.VAR_NAME_POST_TITLE_TILE => TitleTile,
        //     Global.VAR_NAME_POST_URL => Url,
        //     _ => throw new Exception($"Cannot resolve property on post[{name}]")
        // };

    }
}