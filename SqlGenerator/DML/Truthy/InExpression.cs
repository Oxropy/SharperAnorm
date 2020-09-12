namespace SqlGenerator.DML.Truthy
{
    public class InExpression : ITruthy
    {
        public IExpression Lhs { get; }
        public IExpression Rhs { get; }

        public InExpression(IExpression lhs, IExpression rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }
}