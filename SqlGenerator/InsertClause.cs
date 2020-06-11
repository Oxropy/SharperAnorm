using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlGenerator
{
    public class InsertClause : IQueryPart
    {
        private readonly string _table;
        private readonly IEnumerable<(string field, object value)> _values;

        public InsertClause(string table, IEnumerable<(string, object)> values)
        {
            _table = table;
            _values = values;
        }

        public void Build(StringBuilder sb)
        {
            const string seperator = ", ";
            sb.Append("INSERT INTO ");
            sb.Append(_table);
            sb.Append(" (");
            QueryHelper.BuildSeperated(sb, seperator, _values.Select(v => v.field), (part, builder) => builder.Append(part));
            sb.Append(") ");
            sb.Append("VALUES ");
            sb.Append("(");
            QueryHelper.BuildSeperated(sb, seperator, _values.Select(v => v.value), (part, builder) => builder.Append(part));
            sb.Append(")");
        }
    }

    public class InsertStatement : IQuery
    {
        private readonly InsertClause _insert;

        private InsertStatement(InsertClause insert)
        {
            _insert = insert;
        }

        public void Build(StringBuilder sb)
        {
            _insert.Build(sb);
        }

        public static InsertStatement Insert(string table, IEnumerable<(string, object)> values)
        {
            var clause = new InsertClause(table, values);
            return new InsertStatement(clause);
        }
    }
}