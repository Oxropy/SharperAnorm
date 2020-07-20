using System.Collections.Generic;

namespace SqlGenerator
{
    public class GroupByClause : IQueryPart
    {
        public IEnumerable<IExpression> GroupBy { get; }

        public GroupByClause(IEnumerable<IExpression> groupBy)
        {
            GroupBy = groupBy;
        }
    }
}