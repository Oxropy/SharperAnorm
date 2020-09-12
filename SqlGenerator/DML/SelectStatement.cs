using SqlGenerator.DML.Truthy;

#nullable enable
namespace SqlGenerator.DML
{
    public class SelectStatement : IQuery
    {
        public SelectClause Select { get; }
        public FromClause From { get; }
        public WhereClause? Where { get; }
        public GroupByClause? GroupBy { get; }
        public OrderByClause? Orderby { get; }

        public SelectStatement(SelectClause select, FromClause from)
        {
            Select = select;
            From = from;
        }

        private SelectStatement(SelectClause select, FromClause from, WhereClause? where, GroupByClause? groupBy, OrderByClause? orderBy)
        {
            Select = select;
            From = from;
            Where = where;
            GroupBy = groupBy;
            Orderby = orderBy;
        }

        public SelectStatement WithWhere(WhereClause where)
        {
            return new SelectStatement(Select, From, where, GroupBy, Orderby);
        }

        public SelectStatement WithGroupBy(GroupByClause groupBy)
        {
            return new SelectStatement(Select, From, Where, groupBy, Orderby);
        }

        public SelectStatement WithOrderBy(OrderByClause orderBy)
        {
            return new SelectStatement(Select, From, Where, GroupBy, orderBy);
        }
    }
}