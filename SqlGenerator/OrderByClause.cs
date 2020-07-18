using System.Collections.Generic;

namespace SqlGenerator
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public class SortOrderClause : IOrderBy
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
        public IEnumerable<IOrderBy> OrderBy { get; }

        public OrderByClause(IEnumerable<IOrderBy> orderBy)
        {
            OrderBy = orderBy;
        }
    }
}