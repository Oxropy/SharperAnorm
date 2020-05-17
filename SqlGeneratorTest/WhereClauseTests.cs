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
    }
}