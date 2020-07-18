using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class OrderByClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        [Test]
        public void BuildSortOrderClauseAscending()
        {
            var field = new FieldReferenceExpression("Field", "Table");
            var result = new SortOrderClause(field).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table.Field ASC"));
        }

        [Test]
        public void BuildSortOrderClauseDescending()
        {
            var field = new FieldReferenceExpression("Field", "Table");
            var result = new SortOrderClause(field, SortOrder.Descending).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table.Field DESC"));
        }

        [Test]
        public void BuildOrderByClause()
        {
            var field = new FieldReferenceExpression("Field", "Table");
            var orderAsc = new SortOrderClause(field, SortOrder.Ascending);
            var orderDesc = new SortOrderClause(field, SortOrder.Descending);
            var result = new OrderByClause(new[] {orderAsc, orderDesc}).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("ORDER BY Table.Field ASC, Table.Field DESC"));
        }
    }
}