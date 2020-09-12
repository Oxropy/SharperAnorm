namespace SqlGenerator.DML.Truthy
{
    public class IsNullExpression : ITruthy
    {
        public IExpression Expr { get; }

        public IsNullExpression(IExpression expr)
        {
            Expr = expr;
        }
    }
}