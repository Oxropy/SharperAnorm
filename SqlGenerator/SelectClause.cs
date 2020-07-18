#nullable enable
using System.Collections.Generic;

namespace SqlGenerator
{
    public class FieldAliasExpression : ISelection
    {
        public IExpression Expr { get; }
        public string Alias { get; }

        public FieldAliasExpression(IExpression expr, string alias)
        {
            Expr = expr;
            Alias = alias;
        }
    }

    public class SelectClause : IQueryPart
    {
        public IEnumerable<ISelection> Sel { get; }

        public SelectClause(IEnumerable<ISelection> sel)
        {
            Sel = sel;
        }
    }

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