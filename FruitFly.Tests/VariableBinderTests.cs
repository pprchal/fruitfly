using NUnit.Framework;
using fruitfly;
using System.Threading.Tasks;

namespace FruitFly.Tests
{
    [TestFixture]
    public class VariableBinderTests : IVariableSource
    {
        [Test]
        [TestCase("wefwe wkjfwkef {scope:var1} sajehfk ejf{scope2:myvar}", ExpectedResult = "wefwe wkjfwkef a sajehfk ejfb")]
        [TestCase("a b c", ExpectedResult = "a b c")]
        [TestCase("{scope:var1}{scope:var1}{scope:var1}", ExpectedResult = "aaa")]
        [TestCase("{template:menu.html}{scope:var1}{scope:var1}", ExpectedResult = "MENUaa")]
        public string IsReplacementWorking(string content)
        {
            var result =  new VariableBinder().Bind(
                content,
                this
            );

            return result.Result;
        }

        // Task<string> IVariableSource.GetVariableValue(Variable variable)
        Task<string> IVariableSource.GetVariableValue(Variable variable)
        {
            if (variable.Scope == "template" && variable.Name == "menu.html")
            {
                return Task.FromResult("MENU");
            }
            else if (variable.Scope == "scope" && variable.Name == "var1")
            {
                return Task.FromResult("a");
            }
            else if (variable.Scope == "scope2" && variable.Name == "myvar")
            {
                return Task.FromResult("b");
            }

            throw new System.NotImplementedException(variable.ReplaceBlock);
        }
    }
}