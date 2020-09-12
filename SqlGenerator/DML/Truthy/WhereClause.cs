namespace SqlGenerator.DML.Truthy
{
    public class WhereClause : IQueryPart
    {
        public ITruthy Expr { get; }

        public WhereClause(ITruthy expr)
        {
            Expr = expr;
        }
    }
}