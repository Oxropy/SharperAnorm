using SqlGenerator.DML.Truthy;

#nullable enable
namespace SqlGenerator.DML
{
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

        public UpdateStatement WithWhere(WhereClause where)
        {
            return new UpdateStatement(Update, where);
        }
    }
}