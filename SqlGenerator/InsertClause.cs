using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public class InsertValue : IInsert
    {
        public string Name { get; }
        public object Value { get; }

        public InsertValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append(Value);
        }
    }

    public class InsertClause : IQueryPart
    {
        public string Name { get; }
        public IEnumerable<InsertValue> Values { get; }

        public InsertClause(string name, IEnumerable<InsertValue> values)
        {
            Name = name;
            Values = values;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("INSERT INTO ");
            sb.Append(Name);
            sb.Append(" (");

            #region Fieldname build

            using var e = Values.GetEnumerator();
            if (e.MoveNext())
            {
                var v = e.Current;
                sb.Append(v.Name);

                while (e.MoveNext())
                {
                    v = e.Current;
                    sb.Append(", ");
                    sb.Append(v.Name);
                }
            }

            #endregion

            sb.Append(") ");
            sb.Append("  VALUES ");
            sb.Append(" (");
            QueryHelper.BuildJoinedExpression(sb, ", ", Values);
            sb.Append(")");
        }
    }
}