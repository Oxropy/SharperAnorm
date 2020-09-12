namespace SqlGenerator.DML.FieldAndTable
{
    public class TableName : ITableName
    {
        public string Table { get; }
        public string Alias { get; }

        public TableName(string table, string alias = "")
        {
            Table = table;
            Alias = alias;
        }
    }
}