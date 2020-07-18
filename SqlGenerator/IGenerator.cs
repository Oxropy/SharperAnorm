using System.Text;

namespace SqlGenerator
{
    public interface IGenerator
    {
        void Build(IQueryPart queryPart, StringBuilder sb);
        void Build(ISelection selection, StringBuilder sb);
        void Build(ITableName name, StringBuilder sb);
        void Build(IExpression expression, StringBuilder sb);
        void Build(ITruthy truthy, StringBuilder sb);
        void Build(ILiteralExpression literal, StringBuilder sb);
        void Build(IOrderBy orderBy, StringBuilder sb);
        void Build(IGroupBy groupBy, StringBuilder sb);
        void Build(ICreate create, StringBuilder sb);
        void Build(ConnectQueryExpression expression, StringBuilder sb);
        void Build(LiteralExpression expression, StringBuilder sb);
        void Build(FieldReferenceExpression expression, StringBuilder sb);
        void Build(FunctionCallExpression expression, StringBuilder sb);
        void Build(ListExpression expression, StringBuilder sb);
        void Build(BaseTypeColumnDefinition definition, StringBuilder sb);
        void Build(CreateClause clause, StringBuilder sb);
        void Build(DropClause clause, StringBuilder sb);
        void Build(InsertClause clause, StringBuilder sb);
        void Build(InsertStatement statement, StringBuilder sb);
        void Build(DeleteClause clause, StringBuilder sb);
        void Build(DeleteStatement statement, StringBuilder sb);
        void Build(ComparisonExpression expression, StringBuilder sb);
        void Build(Junction junction, StringBuilder sb);
        void Build(Junctions junctions, StringBuilder sb);
        void Build(IsNullExpression expression, StringBuilder sb);
        void Build(InExpression expression, StringBuilder sb);
        void Build(LikeExpression expression, StringBuilder sb);
        void Build(NotExpression expression, StringBuilder sb);
        void Build(PlaceholderExpression expression, StringBuilder sb);
        void Build(WhereClause clause, StringBuilder sb);
        void Build(FieldAliasExpression expression, StringBuilder sb);
        void Build(SelectClause clause, StringBuilder sb);
        void Build(SelectStatement statement, StringBuilder sb);
        void Build(UpdateClause clause, StringBuilder sb);
        void Build(UpdateStatement statement, StringBuilder sb);
        void Build(TableName table, StringBuilder sb);
        void Build(JoinCondition condition, StringBuilder sb);
        void Build(FromClause clause, StringBuilder sb);
        void Build(SortOrderClause clause, StringBuilder sb);
        void Build(OrderByClause clause, StringBuilder sb);
        void Build(GroupByClause clause, StringBuilder sb);
    }
}