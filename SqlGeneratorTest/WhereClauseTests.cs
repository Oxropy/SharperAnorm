using System;
using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class WhereClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        private static IExpression TestExpression => new LiteralExpression("TEST");

        private static ITruthy TestTruthy => new ComparisonExpression(new LiteralExpression("1"), ComparisonOperator.Equal, new LiteralExpression("1"));

        #region Comparison operator

        [Test]
        public void BuildEqual()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.Equal, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' = 'TEST'"));
        }

        [Test]
        public void BuildNotEqual()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.NotEqual, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' != 'TEST'"));
        }

        [Test]
        public void BuildGreaterThan()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.GreaterThan, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' > 'TEST'"));
        }

        [Test]
        public void BuildGreaterThanOrEqual()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.GreaterThanOrEqual, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' >= 'TEST'"));
        }

        [Test]
        public void BuildLowerThan()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.LowerThan, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' < 'TEST'"));
        }

        [Test]
        public void BuildLowerThanOrEqual()
        {
            var exp = TestExpression;
            var result = new ComparisonExpression(exp, ComparisonOperator.LowerThanOrEqual, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' <= 'TEST'"));
        }

        [Test]
        public void BuildCompareUnknownOperator()
        {
            var exp = TestExpression;
            Assert.That(() => new ComparisonExpression(exp, (ComparisonOperator) 10, exp).GetQuery(_generator), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junction

        [Test]
        public void BuildAndExpressions()
        {
            var exp = TestTruthy;
            var result = new Junction(exp, JunctionOp.And, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("('1' = '1') AND ('1' = '1')"));
        }

        [Test]
        public void BuildOrExpressions()
        {
            var exp = TestTruthy;
            var result = new Junction(exp, JunctionOp.Or, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("('1' = '1') OR ('1' = '1')"));
        }

        [Test]
        public void BuildJunctionUnknownOperator()
        {
            var exp = TestTruthy;
            Assert.That(() => new Junction(exp, (JunctionOp) 10, exp).GetQuery(_generator), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junctions

        [Test]
        public void BuildAndsExpressions()
        {
            var exp = TestTruthy;
            var result = new Junctions(JunctionOp.And, exp, exp, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("(('1' = '1') AND ('1' = '1') AND ('1' = '1'))"));
        }

        [Test]
        public void BuildOrsExpressions()
        {
            var exp = TestTruthy;
            var result = new Junctions(JunctionOp.Or, exp, exp, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("(('1' = '1') OR ('1' = '1') OR ('1' = '1'))"));
        }

        [Test]
        public void BuildJunctionsUnknownOperator()
        {
            var exp = TestTruthy;
            Assert.That(() => new Junctions((JunctionOp) 10, exp, exp, exp).GetQuery(_generator), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        [Test]
        public void BuildIsNull()
        {
            var exp = TestTruthy;
            var result = new IsNullExpression(exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("('1' = '1') IS Null"));
        }

        [Test]
        public void BuildIn()
        {
            var exp = TestExpression;
            var result = new InExpression(exp, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' IN ('TEST')"));
        }

        #region Like

        [Test]
        public void BuildLike()
        {
            var exp = TestExpression;
            var result = new LikeExpression(exp, exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("'TEST' LIKE 'TEST'"));
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
            var exp = TestTruthy;
            var result = new NotExpression(exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("NOT ('1' = '1')"));
        }

        [Test]
        public void BuildPlaceholder()
        {
            var result = new PlaceholderExpression().GetQuery(_generator);
            Assert.That(result, Is.EqualTo("?"));
        }

        [Test]
        public void BuildWhereClause()
        {
            var exp = TestTruthy;
            var result = new WhereClause(exp).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("WHERE '1' = '1'"));
        }
    }
}