using System;
using System.Collections.Generic;

namespace SqlGenerator
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public class SortOrderClause : IExpression
    {
        public FieldReferenceExpression Field { get; }
        public SortOrder Sort { get; }

        public SortOrderClause(FieldReferenceExpression field, SortOrder sort = SortOrder.Ascending)
        {
            Field = field;
            Sort = sort;
        }
    }

    public class OrderByClause : IQueryPart
    {
        public IEnumerable<IExpression> OrderBy { get; }

        public OrderByClause(IEnumerable<IExpression> orderBy)
        {
            OrderBy = orderBy;
        }
    }
}