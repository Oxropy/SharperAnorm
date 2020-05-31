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
        private readonly SelectClause _select;
        private readonly FromClause _from;
        private readonly WhereClause? _where;
        private readonly GroupByClause? _groupBy;
        private readonly OrderByClause? _orderby;

        public SelectStatement(SelectClause select, FromClause from)
        {
            _select = select;
            _from = from;
        }
        
        private SelectStatement(SelectClause select, FromClause from, WhereClause? where, GroupByClause? groupBy, OrderByClause? orderBy)
        {
            _select = select;
            _from = from;
            _where = where;
            _groupBy = groupBy;
            _orderby = orderBy;
        }

        public SelectStatement AddWhere(WhereClause where)
        {
            return new SelectStatement(_select, _from, where, _groupBy, _orderby);
        }
        
        public SelectStatement AddGroupBy(GroupByClause groupBy)
        {
            return new SelectStatement(_select, _from, _where, groupBy, _orderby);
        }
        
        public SelectStatement AddOrderBy(OrderByClause orderBy)
        {
            return new SelectStatement(_select, _from, _where, _groupBy, orderBy);
        }
        
        public void Build(StringBuilder sb)
        {
            _select.Build(sb);
            sb.Append(" ");
            _from.Build(sb);
            QueryHelper.AppendQueryPart(sb, _where);
            QueryHelper.AppendQueryPart(sb, _groupBy);
            QueryHelper.AppendQueryPart(sb, _orderby);
        }
    }
}