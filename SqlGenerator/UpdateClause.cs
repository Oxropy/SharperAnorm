#nullable enable
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class UpdateClause : IQueryPart
    {
        private readonly string _table;
        private readonly IEnumerable<(string filed, object value)> _values;

        public UpdateClause(string table, IEnumerable<(string, object)> values)
        {
            _table = table;
            _values = values;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("UPDATE ");
            sb.Append(_table);
            sb.Append(" SET (");
            QueryHelper.BuildSeperated(sb, ", ", _values, (part, builder) =>
            {
                (string field, object value) = part;
                sb.Append(field);
                sb.Append(" = ");
                sb.Append(value);
            });
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