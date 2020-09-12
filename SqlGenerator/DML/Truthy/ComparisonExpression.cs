using System;

namespace SqlGenerator.DML.Truthy
{
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
}