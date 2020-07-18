namespace SqlGenerator
{
    public class DropClause : IQueryPart
    {
        public string Table { get; }

        public DropClause(string table)
        {
            Table = table;
        }
    }
}