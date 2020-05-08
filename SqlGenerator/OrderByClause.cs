using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public enum SortOrder
    {
        Asc,
        Desc
    }

    public class SortOrderClause : IOrderBy
    {
        public FieldReferenceExpression Field { get; }
        public SortOrder Sort { get; }

        public SortOrderClause(FieldReferenceExpression field, SortOrder sort = SortOrder.Asc)
        {
            Field = field;
            Sort = sort;
        }

        public void BuildQuery(StringBuilder sb)
        {
            Field.BuildQuery(sb);
            sb.Append(Sort);
        }
    }

    public class OrderByClause : IQueryPart
    {
        public IEnumerable<IOrderBy> OrderBy { get; }

        public OrderByClause(IEnumerable<IOrderBy> orderBy)
        {
            OrderBy = orderBy;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("ORDER BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", OrderBy);
        }
    }
}