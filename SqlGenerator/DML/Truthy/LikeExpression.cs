namespace SqlGenerator.DML.Truthy
{
    public class LikeExpression : ITruthy
    {
        public IValue Lhs { get; }
        public IValue Rhs { get; }

        public LikeExpression(IValue lhs, IValue rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }
}