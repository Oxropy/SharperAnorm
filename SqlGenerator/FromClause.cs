namespace SqlGenerator
{
    public enum JoinClause
    {
        Inner,
        Left,
        Right,
        Full,
        LeftOuter,
        RightOuter,
        FullOuter
    }

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

    public class JoinCondition : ITableName
    {
        public ITableName Table { get; }
        public JoinClause Join { get; }
        public ITableName TableToJoin { get; }
        public ITruthy Condition { get; }

        public JoinCondition(ITableName table, JoinClause join, ITableName tableToJoin, ITruthy condition)
        {
            Table = table;
            Join = join;
            TableToJoin = tableToJoin;
            Condition = condition;
        }
    }

    public class FromClause : IQueryPart
    {
        public ITableName Table { get; }

        public FromClause(ITableName table)
        {
            Table = table;
        }
    }
}