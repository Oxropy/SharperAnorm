namespace SqlGenerator.DML.FieldAndTable
{
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
}