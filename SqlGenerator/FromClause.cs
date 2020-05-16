using System;
using System.Text;

namespace SqlGenerator
{
    public interface ITableName : ISelection
    {
    }

    public enum JoinClause
    {
        Inner,
        Left,
        Right,
        Full,
        LeftOuter,
        RightOuter,
        FullOuter
    }

    public class TableName : ITableName
    {
        private readonly string _name;
        private readonly string _alias;

        public TableName(string name, string alias = "")
        {
            _name = name;
            _alias = alias;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(_name);
            if (string.IsNullOrWhiteSpace(_alias))
            {
                return;
            }

            sb.Append(" ");
            sb.Append(_alias);
        }
    }

    public class JoinCondition : ITableName
    {
        private readonly ITableName _lhs;
        private readonly JoinClause _jn;
        private readonly ITableName _rhs;
        private readonly ITruthy _comp;

        public JoinCondition(ITableName lhs, JoinClause jn, ITableName rhs, ITruthy comp)
        {
            _lhs = lhs;
            _jn = jn;
            _rhs = rhs;
            _comp = comp;
        }

        public void Build(StringBuilder sb)
        {
            _lhs.Build(sb);
            sb.Append(" ");
            sb.Append(GetJoinValue(_jn));
            sb.Append(" ");
            _rhs.Build(sb);
            sb.Append(" ON (");
            _comp.Build(sb);
            sb.Append(")");
        }

        private static string GetJoinValue(JoinClause jn)
        {
            return jn switch
            {
                JoinClause.Inner => "INNER JOIN",
                JoinClause.Left => "LEFT JOIN",
                JoinClause.Right => "RIGHT JOIN",
                JoinClause.Full => "FULL JOIN",
                JoinClause.LeftOuter => "LEFT OUTER JOIN",
                JoinClause.RightOuter => "RIGHT OUTER JOIN",
                JoinClause.FullOuter => "FULL OUTER JOIN",
                _ => throw new Exception("JoinClause unknown!")
            };
        }
    }

    public class FromClause : IQueryPart
    {
        private readonly ITableName _tbl;

        public FromClause(ITableName tbl)
        {
            _tbl = tbl;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("FROM ");
            _tbl.Build(sb);
        }
    }
}