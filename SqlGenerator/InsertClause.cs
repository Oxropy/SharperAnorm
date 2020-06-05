using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlGenerator
{
    public class InsertValue : FieldValue
    {
        public InsertValue(string field, object value) : base(field, value)
        {
        }
    }
    
    public class InsertClause : IQueryPart
    {
        private readonly string _table;
        private readonly IEnumerable<InsertValue> _values;

        public InsertClause(string table, IEnumerable<InsertValue> values)
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
            QueryHelper.BuildSeperated(sb, seperator, _values.Select(v => v.Field), (part, builder) => builder.Append(part));
            sb.Append(") ");
            sb.Append("VALUES ");
            sb.Append("(");
            QueryHelper.BuildSeperated(sb, seperator, _values.Select(v => v.Value), (part, builder) => builder.Append(part));
            sb.Append(")");
        }
    }

    public class InsertStatement : IQuery
    {
        private readonly InsertStatement _insert;

        public InsertStatement(InsertStatement insert)
        {
            _insert = insert;
        }

        public void Build(StringBuilder sb)
        {
            _insert.Build(sb);
        }
    }
}