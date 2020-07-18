using System.Collections.Generic;

namespace SqlGenerator
{
    public class GroupByClause : IQueryPart
    {
        public IEnumerable<IGroupBy> GroupBy { get; }

        public GroupByClause(IEnumerable<IGroupBy> groupBy)
        {
            GroupBy = groupBy;
        }
    }
}