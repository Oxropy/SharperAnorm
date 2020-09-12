namespace SqlGenerator.DML.Truthy
{
    public class NotExpression : ITruthy
    {
        public ITruthy Expr { get; }

        public NotExpression(ITruthy expr)
        {
            Expr = expr;
        }
    }
}