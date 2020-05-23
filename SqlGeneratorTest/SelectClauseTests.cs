using System.Text;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class SelectClauseTests
    {
        private class TestExpression : IExpression
        {
            public void Build(StringBuilder sb)
            {
                sb.Append("TEST");
            }
        }
        
        [Test]
        public void BuildFieldAlias()
        {
            var exp = new TestExpression();
            var result = new FieldAliasExpression(exp, "test").GetQuery();
            Assert.That(result, Is.EqualTo("(TEST) AS test"));
        }
        
        [Test]
        public void BuildSelectClause()
        {
            var exp = new TestExpression();
            var field1 = new FieldAliasExpression(exp, "test");
            var field2 = new FieldAliasExpression(exp, "Test");
            var result = new SelectClause(new []{field1, field2}).GetQuery();
            Assert.That(result, Is.EqualTo("SELECT (TEST) AS test, (TEST) AS Test"));
        }
    }
}