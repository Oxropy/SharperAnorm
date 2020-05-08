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
        public string Name { get; }
        public string Alias { get; }

        public TableName(string name, string alias = "")
        {
            Name = name;
            Alias = alias;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append(Name);
            if (!string.IsNullOrWhiteSpace(Alias))
            {
                sb.Append(" ");
                sb.Append(Alias);
            }
        }
    }

    public class JoinCondition : ITableName
    {
        public ITableName Lhs { get; }
        public JoinClause Jn { get; }
        public ITableName Rhs { get; }
        public ITruthy Comp { get; }

        public JoinCondition(ITableName lhs, JoinClause jn, ITableName rhs, ITruthy comp)
        {
            Lhs = lhs;
            Jn = jn;
            Rhs = rhs;
            Comp = comp;
        }

        public void BuildQuery(StringBuilder sb)
        {
            Lhs.BuildQuery(sb);
            sb.Append(" ");
            sb.Append(GetJoinValue(Jn));
            sb.Append(" ");
            Rhs.BuildQuery(sb);
            sb.Append(" ON (");
            Comp.BuildQuery(sb);
            sb.Append(")");
        }

        public static string GetJoinValue(JoinClause jn)
        {
            switch (jn)
            {
                case JoinClause.Inner:
                    return "INNER JOIN";
                case JoinClause.Left:
                    return "LEFT JOIN";
                case JoinClause.Right:
                    return "RIGHT JOIN";
                case JoinClause.Full:
                    return "FULL JOIN";
                case JoinClause.LeftOuter:
                    return "LEFT OUTER JOIN";
                case JoinClause.RightOuter:
                    return "RIGHT OUTER JOIN";
                case JoinClause.FullOuter:
                    return "FULL OUTER JOIN";
                default:
                    throw new Exception("JoinClause unknown!");
            }
        }
    }

    public class FromClause : IQueryPart
    {
        public ITableName Tbl { get; }

        public FromClause(ITableName tbl)
        {
            Tbl = tbl;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("FROM ");
            Tbl.BuildQuery(sb);
        }
    }
}