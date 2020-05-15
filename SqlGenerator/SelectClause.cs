using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class FieldAliasExpression : ISelection
    {
        private readonly IExpression _expr;
        private readonly string _alias;

        public FieldAliasExpression(IExpression expr, string alias)
        {
            _expr = expr;
            _alias = alias;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("(");
            _expr.BuildQuery(sb);
            sb.Append(") AS ");
            sb.Append(_alias);
        }
    }

    public class SelectClause : IQueryPart
    {
        private readonly IEnumerable<ISelection> _sel;

        public SelectClause(IEnumerable<ISelection> sel)
        {
            _sel = sel;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("SELECT ");
            QueryHelper.BuildJoinedExpression(sb, ", ", _sel);
        }
    }
}