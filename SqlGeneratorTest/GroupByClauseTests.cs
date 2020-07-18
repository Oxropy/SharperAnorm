using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class GroupByClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        [Test]
        public void BuildGroupByClause()
        {
            var field = new FieldReferenceExpression("Field", "Table");
            var result = new GroupByClause(new[] {field, field}).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("GROUP BY Table.Field, Table.Field"));
        }
    }
}