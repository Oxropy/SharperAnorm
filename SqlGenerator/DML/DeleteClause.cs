using SqlGenerator.DML.FieldAndTable;

namespace SqlGenerator.DML
{
    public class DeleteClause : IQueryPart
    {
        public TableName Table { get; }

        public DeleteClause(TableName table)
        {
            Table = table;
        }
    }
}