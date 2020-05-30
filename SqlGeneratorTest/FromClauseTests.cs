using System.Text;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class FromClauseTests
    {
        private class TestTruthy : ITruthy
        {
            public void Build(StringBuilder sb)
            {
                sb.Append("Test");
            }
        }

        #region Table name

        [Test]
        public void BuildTableNameWithoutAlias()
        {
            var result = new TableName("Table").GetQuery();
            Assert.That(result, Is.EqualTo("Table"));
        }
        
        [Test]
        public void BuildTableNameWithAlias()
        {
            var result = new TableName("Table", "Tbl").GetQuery();
            Assert.That(result, Is.EqualTo("Table Tbl"));
        }

        #endregion

        #region Join condition

        [Test]
        public void BuildInnerJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.Inner, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left INNER JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildLeftJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.Left, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left LEFT JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildRightJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.Right, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left RIGHT JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildFullJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.Full, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left FULL JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildLeftOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.LeftOuter, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left LEFT OUTER JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildRightOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.RightOuter, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left RIGHT OUTER JOIN Right ON (Test)"));
        }
        
        [Test]
        public void BuildFullOuterJoinCondition()
        {
            var lhs = new TableName("Left");
            var rhs = new TableName("Right");
            var where = new TestTruthy();
            var result = new JoinCondition(lhs, JoinClause.FullOuter, rhs, where).GetQuery();
            Assert.That(result, Is.EqualTo("Left FULL OUTER JOIN Right ON (Test)"));
        }

        #endregion
        
        [Test]
        public void BuildFromClause()
        {
            var name = new TableName("Table");
            var result = new FromClause(name).GetQuery();
            Assert.That(result, Is.EqualTo("FROM Table"));
        }
    }
}