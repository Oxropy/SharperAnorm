using System.Collections.Generic;
using SqlGenerator.DDL;

namespace SqlGenerator.DML
{
    public class UpdateClause : TableAndFieldValues
    {
        public UpdateClause(string table, IEnumerable<FieldValue> values) : base(table, values) { }
    }
}