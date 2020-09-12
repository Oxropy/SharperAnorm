using System.Collections.Generic;

namespace SqlGenerator.DML
{
    public class GroupByClause : IQueryPart
    {
        public IEnumerable<IValue> GroupBy { get; }

        public GroupByClause(IEnumerable<IValue> groupBy)
        {
            GroupBy = groupBy;
        }
    }
}