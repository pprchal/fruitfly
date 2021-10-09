// Pavel Prchal, 2019

namespace fruitfly
{
    public class Console : IConsole
    {
        void IConsole.WriteLine(string msg)
        {
            System.Console.WriteLine(msg);
        }
    }
}
