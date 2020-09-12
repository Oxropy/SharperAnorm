namespace SqlGenerator.DML
{
    public class FromClause : IQueryPart
    {
        public ITableName Table { get; }

        public FromClause(ITableName table)
        {
            Table = table;
        }
    }
}