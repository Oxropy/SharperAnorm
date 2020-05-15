using System;
using System.Collections.Generic;
using System.Text;

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

    public class Junction : ITruthy
    {
        private readonly ITruthy _lhs;
        private readonly JunctionOp _op;
        private readonly ITruthy _rhs;

        public Junction(ITruthy lhs, JunctionOp op, ITruthy rhs)
        {
            _lhs = lhs;
            _op = op;
            _rhs = rhs;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("(");
            _lhs.BuildQuery(sb);
            sb.Append(") ");
            sb.Append(_op);
            sb.Append(" (");
            _rhs.BuildQuery(sb);
            sb.Append(")");
        }
    }

    public class Junctions : ITruthy
    {
        private readonly JunctionOp _op;
        private readonly IEnumerable<ITruthy> _truthies;

        public Junctions(JunctionOp op, params ITruthy[] truthies)
        {
            _op = op;
            _truthies = truthies;
        }

        public void BuildQuery(StringBuilder sb)
        {
            string op = _op switch
            {
                JunctionOp.And => "AND",
                JunctionOp.Or => "OR",
                _ => throw new ArgumentOutOfRangeException()
            };
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, op, _truthies);
            sb.Append(")");
        }
    }

    public class IsNullExpression : ITruthy
    {
        private readonly IExpression _expr;

        public IsNullExpression(IExpression expr)
        {
            _expr = expr;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("(");
            _expr.BuildQuery(sb);
            sb.Append(") IS Null");
        }
    }

    public class InExpression : ITruthy
    {
        private readonly IExpression _lhr;
        private readonly IExpression _rhr;

        public InExpression(IExpression lhr, IExpression rhr)
        {
            _lhr = lhr;
            _rhr = rhr;
        }

        public void BuildQuery(StringBuilder sb)
        {
            _lhr.BuildQuery(sb);
            sb.Append(" IN ");
            sb.Append("(");
            _rhr.BuildQuery(sb);
            sb.Append(")");
        }
    }

    public class LikeExpression : ITruthy
    {
        private readonly IExpression _lhr;
        private readonly IExpression _rhs;

        public LikeExpression(IExpression lhr, IExpression rhs)
        {
            _lhr = lhr;
            _rhs = rhs;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append(_lhr);
            sb.Append(" LIKE ");
            if (_rhs is LiteralExpression rhs)
            {
                if (rhs.Literal is string)
                {
                    _rhs.BuildQuery(sb);
                }
                else
                {
                    sb.Append("'");
                    _rhs.BuildQuery(sb);
                    sb.Append("'");
                }
            }
            else
            {
                _rhs.BuildQuery(sb);
            }
        }
    }

    public class ComparisonExpression : ITruthy
    {
        private readonly IExpression _lhs;
        private readonly ComparisonOperator _op;
        private readonly IExpression _rhs;

        public ComparisonExpression(IExpression lhs, ComparisonOperator op, IExpression rhs)
        {
            _lhs = lhs;
            _op = op;
            _rhs = rhs;
        }

        public void BuildQuery(StringBuilder sb)
        {
            _lhs.BuildQuery(sb);
            sb.Append(" ");
            sb.Append(GetOperatorValue(_op));
            sb.Append(" ");
            _rhs.BuildQuery(sb);
        }

        private static string GetOperatorValue(ComparisonOperator op)
        {
            return op switch
            {
                ComparisonOperator.Equal => "=",
                ComparisonOperator.NotEqual => "!=",
                ComparisonOperator.GreaterThan => ">",
                ComparisonOperator.GreaterThanOrEqual => ">=",
                ComparisonOperator.LowerThan => "<",
                ComparisonOperator.LowerThanOrEqual => "<=",
                _ => throw new Exception("Unknown operator!")
            };
        }
    }

    public class NotExpression : ITruthy
    {
        private readonly ITruthy _expr;

        public NotExpression(ITruthy expr)
        {
            _expr = expr;
        }
        
        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("NOT (");
            _expr.BuildQuery(sb);
            sb.Append(")");
        }
    }

    public class ListExpression : IExpression
    {
        private readonly IEnumerable<IExpression> _expressions;

        public ListExpression(IEnumerable<IExpression> expressions)
        {
            _expressions = expressions;
        }

        public void BuildQuery(StringBuilder sb)
        {
            QueryHelper.BuildJoinedExpression(sb, ", ", _expressions);
        }
    }

    public class PlaceholderExpression : IExpression
    {
        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("?");
        }
    }

    public class WhereClause : IQueryPart
    {
        private readonly ITruthy _expr;

        public WhereClause(ITruthy expr)
        {
            _expr = expr;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("WHERE ");
            _expr.BuildQuery(sb);
        }
    }
}