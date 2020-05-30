using System;
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

        public void Build(StringBuilder sb)
        {
            _field.Build(sb);
            sb.Append(" ");
            sb.Append(GetSortOrderValue(_sort));
        }
        
        private static string GetSortOrderValue(SortOrder field)
        {
            return field switch
            {
                SortOrder.Ascending => "ASC",
                SortOrder.Descending => "DESC",
                _ => throw new NotSupportedException("Unknown order!")
            };
        }
    }

    public class OrderByClause : IQueryPart
    {
        private readonly IEnumerable<IOrderBy> _orderBy;

        public OrderByClause(IEnumerable<IOrderBy> orderBy)
        {
            _orderBy = orderBy;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("ORDER BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", _orderBy);
        }
    }
}