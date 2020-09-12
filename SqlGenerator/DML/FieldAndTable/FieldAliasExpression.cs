namespace SqlGenerator.DML.FieldAndTable
{
    public class FieldAliasExpression : ISelection
    {
        public IExpression Expr { get; }
        public string Alias { get; }

        public FieldAliasExpression(IExpression expr, string alias)
        {
            Expr = expr;
            Alias = alias;
        }
    }
}