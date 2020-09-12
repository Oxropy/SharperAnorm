using System.Collections.Generic;

namespace SqlGenerator.DDL
{
    public class TableAndFieldValues : IQueryPart
    {
        public string Table { get; }
        public IEnumerable<FieldValue> Values { get; }

        protected TableAndFieldValues(string table, IEnumerable<FieldValue> values)
        {
            Table = table;
            Values = values;
        }
    }
}