using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class GroupByClauseTests
    {
        [Test]
        public void BuildGroupByClause()
        {
            var field = new FieldReferenceExpression("Field", "Table");
            var result = new GroupByClause(new []{field, field}).GetQuery();
            Assert.That(result, Is.EqualTo("GROUP BY Table.Field, Table.Field"));
        }
    }
}