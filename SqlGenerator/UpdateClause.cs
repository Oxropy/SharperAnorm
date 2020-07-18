#nullable enable
using System.Collections.Generic;

namespace SqlGenerator
{
    public class UpdateClause : IQueryPart
    {
        public string Table { get; }
        public IEnumerable<(string filed, object value)> Values { get; }

        public UpdateClause(string table, IEnumerable<(string, object)> values)
        {
            Table = table;
            Values = values;
        }
    }

    public class UpdateStatement : IQuery
    {
        public UpdateClause Update { get; }
        public WhereClause? Where { get; }

        public UpdateStatement(UpdateClause update)
        {
            Update = update;
        }

        private UpdateStatement(UpdateClause update, WhereClause where)
        {
            Update = update;
            Where = where;
        }

        public UpdateStatement AddWhere(WhereClause where)
        {
            return new UpdateStatement(Update, where);
        }
    }
}