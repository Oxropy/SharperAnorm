using System.Text;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class QueryBuilderTests
    {
        private class TestExpression : IExpression
        {
            public void Build(StringBuilder sb)
            {
                sb.Append("TEST");
            }
        }

        private class TestTruthy : ITruthy
        {
            public void Build(StringBuilder sb)
            {
                sb.Append("Test");
            }
        }

        #region Field Reference

        [Test]
        public void BuildFieldReferenceWithoutTable()
        {
            var result = new FieldReferenceExpression("Name").GetQuery();
            Assert.That(result, Is.EqualTo("Name"));
        }

        [Test]
        public void BuildFieldReferenceWithTable()
        {
            var result = new FieldReferenceExpression("Name", "Table").GetQuery();
            Assert.That(result, Is.EqualTo("Table.Name"));
        }

        #endregion

        #region Function Call

        [Test]
        public void BuildFunctionCall()
        {
            var result = new FunctionCallExpression("Name", new[] {new TestExpression()}).GetQuery();
            Assert.That(result, Is.EqualTo("Name(TEST)"));
        }

        #endregion

        #region Builder Extensions

        [Test]
        public void CreateFunctionCall()
        {
            var result = "Name".Call(new TestExpression()).GetQuery();
            Assert.That(result, Is.EqualTo("Name(TEST)"));
        }

        [Test]
        public void CreateFieldReferenceWithoutTable()
        {
            var result = "Name".Col().GetQuery();
            Assert.That(result, Is.EqualTo("Name"));
        }

        [Test]
        public void CreateFieldReferenceWithTable()
        {
            var result = "Name".Col("Table").GetQuery();
            Assert.That(result, Is.EqualTo("Table.Name"));
        }

        #region Build

        [Test]
        public void AddFromToSelectClause()
        {
            var select = new SelectClause(new[] {"Name".Col()});
            var from = new FromClause(new TableName("Table"));
            var result = select.AddFrom(from).GetQuery();
            Assert.That(result, Is.EqualTo("SELECT Name FROM Table"));
        }

        [Test]
        public void AddWhereToDeleteClause()
        {
            var delete = new DeleteClause("Table");
            var where = new WhereClause(new TestTruthy());
            var result = delete.AddWhere(where).GetQuery();
            Assert.That(result, Is.EqualTo("DELETE FROM Table WHERE Test"));
        }

        [Test]
        public void AddOrderByToWhereClause()
        {
            var where = new WhereClause(new TestTruthy());
            var orderBy = new OrderByClause(new[] {new SortOrderClause("Field".Col())});
            var result = where.AddOrderBy(orderBy).GetQuery();
            Assert.That(result, Is.EqualTo("WHERE Test ORDER BY Field ASC"));
        }

        [Test]
        public void AddGroupByToWhereClause()
        {
            var where = new WhereClause(new TestTruthy());
            var groupBy = new GroupByClause(new[] {"Field".Col()});
            var result = where.AddGroupBy(groupBy).GetQuery();
            Assert.That(result, Is.EqualTo("WHERE Test GROUP BY Field"));
        }
        
        #endregion

        #endregion

        #region Query Helper

        [Test]
        public void BuildJoinExpressionWithoutValuesWithout()
        {
            var sb = new StringBuilder();
            QueryHelper.BuildJoinedExpression(sb, ",", new IQueryPart[0]);
            Assert.That(sb.ToString(), Is.Empty);
        }

        #endregion
    }
}