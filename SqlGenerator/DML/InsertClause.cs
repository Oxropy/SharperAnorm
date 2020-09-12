using System.Collections.Generic;
using SqlGenerator.DDL;

namespace SqlGenerator.DML
{
    public class InsertClause : TableAndFieldValues
    {
        public InsertClause(string table, IEnumerable<FieldValue> values) : base(table, values) { }
    }

    public class InsertStatement : IQuery
    {
        public InsertClause InsertClause { get; }

        private InsertStatement(InsertClause insertClause)
        {
            InsertClause = insertClause;
        }

        public static InsertStatement Insert(string table, IEnumerable<FieldValue> values)
        {
            var clause = new InsertClause(table, values);
            return new InsertStatement(clause);
        }
    }
}