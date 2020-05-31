#nullable enable
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class FieldAliasExpression : ISelection
    {
        private readonly IExpression _expr;
        private readonly string _alias;

        public FieldAliasExpression(IExpression expr, string alias)
        {
            _expr = expr;
            _alias = alias;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("(");
            _expr.Build(sb);
            sb.Append(") AS ");
            sb.Append(_alias);
        }
    }

    public class SelectClause : IQueryPart
    {
        private readonly IEnumerable<ISelection> _sel;

        public SelectClause(IEnumerable<ISelection> sel)
        {
            _sel = sel;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("SELECT ");
            QueryHelper.BuildJoinedExpression(sb, ", ", _sel);
        }
    }
    
    public class SelectStatement : IQuery
    {
        private readonly SelectClause _update;
        private readonly WhereClause? _where;
        private readonly GroupByClause? _groupBy;
        private readonly OrderByClause? _orderby;

        public SelectStatement(SelectClause update)
        {
            _update = update;
        }
        
        private SelectStatement(SelectClause update, WhereClause? where, GroupByClause? groupBy, OrderByClause? orderBy)
        {
            _update = update;
            _where = where;
            _groupBy = groupBy;
            _orderby = orderBy;
        }

        public SelectStatement AddWhere(WhereClause where)
        {
            return new SelectStatement(_update, where, _groupBy, _orderby);
        }
        
        public SelectStatement AddGroupBy(GroupByClause groupBy)
        {
            return new SelectStatement(_update, _where, groupBy, _orderby);
        }
        
        public SelectStatement AddOrderBy(OrderByClause orderBy)
        {
            return new SelectStatement(_update, _where, _groupBy, orderBy);
        }
        
        public void Build(StringBuilder sb)
        {
            _update.Build(sb);
            QueryHelper.AppendQueryPart(sb, _where);
            QueryHelper.AppendQueryPart(sb, _groupBy);
            QueryHelper.AppendQueryPart(sb, _orderby);
        }
    }
}