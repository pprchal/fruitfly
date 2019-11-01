using fruitfly.core;
using fruitfly.objects;

namespace FruitFly.Tests
{
    public class TestContext : Context
    {
        public override Configuration CreateConfig()
        {
            return new Configuration()
            {
                template = "default"
            };
        }
    }
}