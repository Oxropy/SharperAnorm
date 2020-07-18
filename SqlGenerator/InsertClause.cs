using System.Collections.Generic;

namespace SqlGenerator
{
    public class InsertClause : IQueryPart
    {
        public string Table { get; }
        public IEnumerable<(string field, object value)> Values { get; }

        public InsertClause(string table, IEnumerable<(string, object)> values)
        {
            Table = table;
            Values = values;
        }
    }

    public class InsertStatement : IQuery
    {
        public InsertClause InsertClause { get; }

        private InsertStatement(InsertClause insertClause)
        {
            InsertClause = insertClause;
        }

        public static InsertStatement Insert(string table, IEnumerable<(string, object)> values)
        {
            var clause = new InsertClause(table, values);
            return new InsertStatement(clause);
        }
    }
}