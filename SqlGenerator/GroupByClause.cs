using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class GroupByClause : IQueryPart
    {
        private readonly IEnumerable<IGroupBy> _groupBy;

        public GroupByClause(params IGroupBy[] groupBy)
        {
            _groupBy = groupBy;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("GROUP BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", _groupBy);
        }
    }
}