using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class FromClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        private static ITruthy TestTruthy => new ComparisonExpression(new LiteralExpression("1"), ComparisonOperator.Equal, new LiteralExpression("1"));

        #region Table name

        [Test]
        public void BuildTableNameWithoutAlias()
        {
            var result = new TableName("Table").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table"));
        }

        [Test]
        public void BuildTableNameWithAlias()
        {
            var result = new TableName("Table", "Tbl").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Table Tbl"));
        }

        #endregion

        #region Join condition

        [Test]
        public void BuildInnerJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.Inner, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left INNER JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildLeftJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.Left, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left LEFT JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildRightJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.Right, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left RIGHT JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildFullJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.Full, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left FULL JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildLeftOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.LeftOuter, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left LEFT OUTER JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildRightOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.RightOuter, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left RIGHT OUTER JOIN Right ON ('1' = '1')"));
        }

        [Test]
        public void BuildFullOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = TestTruthy;
            var result = new JoinCondition(lhs, JoinClause.FullOuter, rhs, where).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("Left FULL OUTER JOIN Right ON ('1' = '1')"));
        }

        #endregion

        [Test]
        public void BuildFromClause()
        {
            var name = new TableName("Table");
            var result = new FromClause(name).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("FROM Table"));
        }
    }
}