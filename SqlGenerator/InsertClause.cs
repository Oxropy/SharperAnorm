using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class InsertValue : IInsert
    {
        public string Field { get; }
        public object Value { get; }

        public InsertValue(string field, object value)
        {
            Field = field;
            Value = value;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(Value);
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
            sb.Append("INSERT INTO ");
            sb.Append(_table);
            sb.Append(" (");

            #region Fieldname build

            using var e = _values.GetEnumerator();
            if (e.MoveNext())
            {
                var v = e.Current;
                sb.Append(v.Field);

                while (e.MoveNext())
                {
                    v = e.Current;
                    sb.Append(", ");
                    sb.Append(v.Field);
                }
            }

            #endregion

            sb.Append(") ");
            sb.Append("VALUES ");
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, ", ", _values);
            sb.Append(")");
        }
    }
}