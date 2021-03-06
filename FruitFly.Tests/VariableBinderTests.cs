﻿using NUnit.Framework;
using fruitfly.core;

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
        public string IsBasicReplacementWorking(string content)
        {
            return new VariableBinder().Bind(
                content,
                this
            ).ToString();
        }

        string IVariableSource.GetVariableValue(Variable variable)
        {
            if (variable.Scope == "template" && variable.Name == "menu.html")
            {
                return "MENU";
            }
            else if (variable.Scope == "scope" && variable.Name == "var1")
            {
                return "a";
            }
            else if (variable.Scope == "scope2" && variable.Name == "myvar")
            {
                return "b";
            }

            throw new System.NotImplementedException(variable.ReplaceBlock);
        }
    }
}