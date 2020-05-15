using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public class SortOrderClause : IOrderBy
    {
        private readonly FieldReferenceExpression _field;
        private readonly SortOrder _sort;

        public SortOrderClause(FieldReferenceExpression field, SortOrder sort = SortOrder.Ascending)
        {
            _field = field;
            _sort = sort;
        }

        public void BuildQuery(StringBuilder sb)
        {
            _field.BuildQuery(sb);
            sb.Append(_sort);
        }
    }

    public class OrderByClause : IQueryPart
    {
        private readonly IEnumerable<IOrderBy> _orderBy;

        public OrderByClause(IEnumerable<IOrderBy> orderBy)
        {
            _orderBy = orderBy;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("ORDER BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", _orderBy);
        }
    }
}