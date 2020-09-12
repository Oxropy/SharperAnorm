using SqlGenerator.DML.Truthy;

#nullable enable
namespace SqlGenerator.DML
{
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

        public DeleteStatement WithWhere(WhereClause where)
        {
            return new DeleteStatement(Delete, where);
        }
    }
}