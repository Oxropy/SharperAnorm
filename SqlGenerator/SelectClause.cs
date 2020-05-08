using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class FieldAliasExpression : ISelection
    {
        public IExpression Expr { get; }
        public string Alias { get; }

        public FieldAliasExpression(IExpression expr, string alias)
        {
            Expr = expr;
            Alias = alias;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("(");
            Expr.BuildQuery(sb);
            sb.Append(") AS ");
            sb.Append(Alias);
        }
    }

    public class SelectClause : IQueryPart
    {
        public IEnumerable<ISelection> Sel { get; }

        public SelectClause(IEnumerable<ISelection> sel)
        {
            Sel = sel;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("SELECT ");
            QueryHelper.BuildJoinedExpression(sb, ", ", Sel);
        }
    }
}