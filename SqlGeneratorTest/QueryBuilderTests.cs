using System.Text;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class QueryBuilderTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        private static IExpression TestExpression => new LiteralExpression("TEST");
        private static ITruthy TestTruthy => new ComparisonExpression(new LiteralExpression("1"), ComparisonOperator.Equal, new LiteralExpression("1"));

        #region Field Reference

        [Test]
        public void BuildFieldReferenceWithoutTable()
        {
            var result = new FieldReferenceExpression("Name").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Name"));
        }

        [Test]
        public void BuildFieldReferenceWithTable()
        {
            var result = new FieldReferenceExpression("Name", "Table").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table.Name"));
        }

        #endregion

        #region Function Call

        [Test]
        public void BuildFunctionCall()
        {
            var result = new FunctionCallExpression("Name", new[] {TestExpression}).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Name('TEST')"));
        }

        #endregion

        #region Builder Extensions

        [Test]
        public void CreateFunctionCall()
        {
            var result = "Name".Call(TestExpression).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Name('TEST')"));
        }

        [Test]
        public void CreateFieldReferenceWithoutTable()
        {
            var result = "Name".Col().GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Name"));
        }

        [Test]
        public void CreateFieldReferenceWithTable()
        {
            var result = "Name".Col("Table").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table.Name"));
        }

        #region Build

        [Test]
        public void AddFromToSelectClause()
        {
            var select = new SelectClause(new[] {"Name".Col()});
            var from = new FromClause(new TableName("Table"));
            var result = select.AddFrom(from).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("SELECT Name FROM Table"));
        }

        [Test]
        public void AddWhereToDeleteClause()
        {
            var delete = new DeleteClause("Table");
            var where = new WhereClause(TestTruthy);
            var result = delete.AddWhere(where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("DELETE FROM Table WHERE '1' = '1'"));
        }

        [Test]
        public void AddOrderByToWhereClause()
        {
            var where = new WhereClause(TestTruthy);
            var orderBy = new OrderByClause(new[] {new SortOrderClause("Field".Col())});
            var result = where.AddOrderBy(orderBy).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("WHERE '1' = '1' ORDER BY Field ASC"));
        }

        [Test]
        public void AddGroupByToWhereClause()
        {
            var where = new WhereClause(TestTruthy);
            var groupBy = new GroupByClause(new[] {"Field".Col()});
            var result = where.AddGroupBy(groupBy).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("WHERE '1' = '1' GROUP BY Field"));
        }

        #endregion

        #endregion

        #region Query Helper

        [Test]
        public void BuildJoinExpressionWithoutValuesWithout()
        {
            var sb = new StringBuilder();
            QueryHelper.BuildJoinedExpression(sb, (string) ",", new IQueryPart[0], _generator);
            Assert.That(sb.ToString(), Is.Empty);
        }

        #endregion
    }
}