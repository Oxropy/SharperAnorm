using System;
using System.Collections.Generic;

namespace SqlGenerator
{
    public enum JunctionOp
    {
        And,
        Or
    }

    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LowerThan,
        LowerThanOrEqual
    }

    public class ComparisonExpression : ITruthy
    {
        public IExpression Lhs { get; }
        public ComparisonOperator Op { get; }
        public IExpression Rhs { get; }

        public ComparisonExpression(IExpression lhs, ComparisonOperator op, IExpression rhs)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }


        public static string GetOperatorValue(ComparisonOperator op)
        {
            return op switch
            {
                ComparisonOperator.Equal => "=",
                ComparisonOperator.NotEqual => "!=",
                ComparisonOperator.GreaterThan => ">",
                ComparisonOperator.GreaterThanOrEqual => ">=",
                ComparisonOperator.LowerThan => "<",
                ComparisonOperator.LowerThanOrEqual => "<=",
                _ => throw new NotSupportedException("Unknown operator!")
            };
        }
    }

    public class Junction : ITruthy
    {
        public ITruthy Lhs { get; }
        public JunctionOp Op { get; }
        public ITruthy Rhs { get; }

        public Junction(ITruthy lhs, JunctionOp op, ITruthy rhs)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }


        public static string GetOperatorValue(JunctionOp op)
        {
            return op switch
            {
                JunctionOp.And => "AND",
                JunctionOp.Or => "OR",
                _ => throw new NotSupportedException("Unknown operator!")
            };
        }
    }

    public class Junctions : ITruthy
    {
        public JunctionOp Op { get; }
        public IEnumerable<ITruthy> Truthies { get; }

        public Junctions(JunctionOp op, params ITruthy[] truthies)
        {
            Op = op;
            Truthies = truthies;
        }
    }

    public class IsNullExpression : ITruthy
    {
        public IExpression Expr { get; }

        public IsNullExpression(IExpression expr)
        {
            Expr = expr;
        }
    }

    public class InExpression : ITruthy
    {
        public IExpression Lhr { get; }
        public IExpression Rhr { get; }

        public InExpression(IExpression lhr, IExpression rhr)
        {
            Lhr = lhr;
            Rhr = rhr;
        }
    }

    public class LikeExpression : ITruthy
    {
        public IExpression Lhr { get; }
        public IExpression Rhs { get; }

        public LikeExpression(IExpression lhr, IExpression rhs)
        {
            Lhr = lhr;
            Rhs = rhs;
        }
    }

    public class NotExpression : ITruthy
    {
        public ITruthy Expr { get; }

        public NotExpression(ITruthy expr)
        {
            Expr = expr;
        }
    }

    public class PlaceholderExpression : IExpression
    {
    }

    public class WhereClause : IQueryPart
    {
        public ITruthy Expr { get; }

        public WhereClause(ITruthy expr)
        {
            Expr = expr;
        }
    }
}