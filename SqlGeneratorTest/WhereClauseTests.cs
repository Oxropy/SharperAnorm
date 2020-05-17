using System;
using System.Text;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class WhereClauseTests
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

        #region Comparison operator

        [Test]
        public void BuildEqual()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.Equal, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST = TEST"));
        }

        [Test]
        public void BuildNotEqual()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.NotEqual, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST != TEST"));
        }

        [Test]
        public void BuildGreaterThan()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.GreaterThan, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST > TEST"));
        }

        [Test]
        public void BuildGreaterThanOrEqual()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.GreaterThanOrEqual, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST >= TEST"));
        }

        [Test]
        public void BuildLowerThan()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.LowerThan, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST < TEST"));
        }

        [Test]
        public void BuildLowerThanOrEqual()
        {
            var exp = new TestExpression();
            var result = new ComparisonExpression(exp, ComparisonOperator.LowerThanOrEqual, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST <= TEST"));
        }

        [Test]
        public void BuildCompareUnknownOperator()
        {
            var exp = new TestExpression();
            Assert.That(() => new ComparisonExpression(exp, (ComparisonOperator) 10, exp).GetQuery(), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junction

        [Test]
        public void BuildAndExpressions()
        {
            var exp = new TestTruthy();
            var result = new Junction(exp, JunctionOp.And, exp).GetQuery();
            Assert.That(result, Is.EqualTo("(Test) AND (Test)"));
        }

        [Test]
        public void BuildOrExpressions()
        {
            var exp = new TestTruthy();
            var result = new Junction(exp, JunctionOp.Or, exp).GetQuery();
            Assert.That(result, Is.EqualTo("(Test) OR (Test)"));
        }

        [Test]
        public void BuildJunctionUnknownOperator()
        {
            var exp = new TestTruthy();
            Assert.That(() => new Junction(exp, (JunctionOp) 10, exp).GetQuery(), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junctions

        [Test]
        public void BuildAndsExpressions()
        {
            var exp = new TestTruthy();
            var result = new Junctions(JunctionOp.And, exp, exp, exp).GetQuery();
            Assert.That(result, Is.EqualTo("((Test) AND (Test) AND (Test))"));
        }

        [Test]
        public void BuildOrsExpressions()
        {
            var exp = new TestTruthy();
            var result = new Junctions(JunctionOp.Or, exp, exp, exp).GetQuery();
            Assert.That(result, Is.EqualTo("((Test) OR (Test) OR (Test))"));
        }

        [Test]
        public void BuildJunctionsUnknownOperator()
        {
            var exp = new TestTruthy();
            Assert.That(() => new Junctions((JunctionOp) 10, exp, exp, exp).GetQuery(), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        [Test]
        public void BuildIsNull()
        {
            var exp = new TestTruthy();
            var result = new IsNullExpression(exp).GetQuery();
            Assert.That(result, Is.EqualTo("(Test) IS Null"));
        }

        [Test]
        public void BuildIn()
        {
            var exp = new TestExpression();
            var result = new InExpression(exp, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST IN (TEST)"));
        }

        #region Like

        [Test]
        public void BuildLike()
        {
            var exp = new TestExpression();
            var result = new LikeExpression(exp, exp).GetQuery();
            Assert.That(result, Is.EqualTo("TEST LIKE TEST"));
        }

        [Test]
        public void BuildLikeWithLierteral()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void BuildLikeWithStringLiteral()
        {
            throw new NotImplementedException();
        }

        #endregion

        [Test]
        public void BuildNot()
        {
            var exp = new TestTruthy();
            var result = new NotExpression(exp).GetQuery();
            Assert.That(result, Is.EqualTo("NOT (Test)"));
        }

        [Test]
        public void BuildPlaceholder()
        {
            var result = new PlaceholderExpression().GetQuery();
            Assert.That(result, Is.EqualTo("?"));
        }

        [Test]
        public void BuildWhereClause()
        {
            var exp = new TestTruthy();
            var result = new WhereClause(exp).GetQuery();
            Assert.That(result, Is.EqualTo("WHERE Test"));
        }
    }
}