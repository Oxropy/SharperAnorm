#nullable enable
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class UpdateValue : IUpdate
    {
        private readonly string _field;
        private readonly object _value;

        public UpdateValue(string field, object value)
        {
            _field = field;
            _value = value;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(_field);
            sb.Append(" = ");
            sb.Append(_value);
        }
    }
    
    public class UpdateClause : IQueryPart
    {
        private readonly string _table;
        private readonly IEnumerable<UpdateValue> _values;

        public UpdateClause(string table, IEnumerable<UpdateValue> values)
        {
            _table = table;
            _values = values;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("UPDATE ");
            sb.Append(_table);
            sb.Append(" SET (");
            QueryHelper.BuildJoinedExpression(sb, ", ", _values);
            sb.Append(")");
        }
    }
    
    public class UpdateStatement : IQuery
    {
        private readonly UpdateClause _update;
        private readonly WhereClause? _where;

        public UpdateStatement(UpdateClause update)
        {
            _update = update;
        }
        
        private UpdateStatement(UpdateClause update, WhereClause where)
        {
            _update = update;
            _where = where;
        }

        public UpdateStatement AddWhere(WhereClause where)
        {
            return new UpdateStatement(_update, where);
        }
        
        public void Build(StringBuilder sb)
        {
            _update.Build(sb);
            QueryHelper.AppendQueryPart(sb, _where);
        }
    }
}