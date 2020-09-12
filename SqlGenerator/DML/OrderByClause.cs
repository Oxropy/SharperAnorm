using System.Collections.Generic;

namespace SqlGenerator.DML
{
    public class OrderByClause : IQueryPart
    {
        public IEnumerable<SortOrderClause> OrderBy { get; }

        public OrderByClause(IEnumerable<SortOrderClause> orderBy)
        {
            OrderBy = orderBy;
        }
    }
}