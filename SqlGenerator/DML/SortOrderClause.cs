namespace SqlGenerator.DML
{
    public class SortOrderClause : IExpression
    {
        public IValue Value { get; }
        public SortOrder Sort { get; }

        public SortOrderClause(IValue value, SortOrder sort = SortOrder.Ascending)
        {
            Value = value;
            Sort = sort;
        }
    }
}