#nullable enable
namespace SqlGenerator
{
    public class DeleteClause : IQueryPart
    {
        public string Table { get; }

        public DeleteClause(string table)
        {
            Table = table;
        }
    }

    public class DeleteStatement : IQuery
    {
        public DeleteClause Delete { get; }
        public WhereClause? Where { get; }

        public DeleteStatement(DeleteClause delete)
        {
            Delete = delete;
        }

        private DeleteStatement(DeleteClause delete, WhereClause where)
        {
            Delete = delete;
            Where = where;
        }

        public DeleteStatement AddWhere(WhereClause where)
        {
            return new DeleteStatement(Delete, where);
        }
    }
}