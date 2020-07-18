using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class SelectClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        private static IExpression TestExpression => new LiteralExpression("TEST");

        [Test]
        public void BuildFieldAlias()
        {
            var exp = TestExpression;
            var result = new FieldAliasExpression(exp, "test").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("('TEST') AS test"));
        }

        [Test]
        public void BuildSelectClause()
        {
            var exp = TestExpression;
            var field1 = new FieldAliasExpression(exp, "test");
            var field2 = new FieldAliasExpression(exp, "Test");
            var result = new SelectClause(new[] {field1, field2}).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("SELECT ('TEST') AS test, ('TEST') AS Test"));
        }
    }
}